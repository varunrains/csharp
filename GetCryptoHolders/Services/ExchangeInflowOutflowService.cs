using Dapper;
using GetCryptoHolders.Dtos;
using GetCryptoHolders.Models;
using Microsoft.Extensions.Configuration;
using Polly;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace GetCryptoHolders.Services
{
    public class ExchangeInflowOutflowService
    {
        public IConfiguration Configuration { get; set; }
        private WhaleAlert latestWhaleAlert = null;
        private string lastCursor = string.Empty;
        public ExchangeInflowOutflowService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private async Task SetTodaysData()
        {
            using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = @"SELECT  [Id]
                                      ,[Cursor]
                                      ,[CreatedDate]
                                      ,[JsonDetail]
                                  FROM [dbo].[WhaleAlerts] WHERE CAST([CreatedDate] as [date]) = @Date";

                var data = await connection.QueryAsync<WhaleAlert>(query, new { Date = DateTime.UtcNow.Date.ToString("yyyy-MM-dd") }).ConfigureAwait(false);
                latestWhaleAlert = data?.FirstOrDefault();
                if (latestWhaleAlert == null)
                {
                    //This extra call is required to get the last valid cursor to get the details
                    var lastCursorData = await connection.QueryAsync<WhaleAlert>(query, new { Date = DateTime.UtcNow.Date.AddDays(-1) }).ConfigureAwait(false);
                    lastCursor = lastCursorData?.FirstOrDefault()?.Cursor;
                }
            }
        }

        public async Task UpdateExchangeInflowOutFlow()
        {
            await SetTodaysData().ConfigureAwait(false);
            var unixTime = DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds();

            var token = Configuration.GetSection("HeaderKeys:WhaleAlertKey")?.Value;
            var min_value = Configuration.GetSection("HeaderKeys:MinimumValue")?.Value;
            var whaleAlertUrl = Configuration.GetSection("HeaderKeys:WhaleAlertUrl")?.Value;
            var cursorToUse = string.IsNullOrWhiteSpace(lastCursor) ? (string.IsNullOrWhiteSpace(latestWhaleAlert?.Cursor) ? null : latestWhaleAlert?.Cursor) : lastCursor;
            string baseUrl = string.IsNullOrWhiteSpace(cursorToUse) ? $"{whaleAlertUrl}?min_value={min_value}&start={unixTime}"
                : $"{whaleAlertUrl}?min_value={min_value}&cursor={cursorToUse}";
           
            var restClient = new RestClient(baseUrl);
            var request = new RestRequest()
            {
                Method = Method.GET,
            };
            request.AddHeader("X-WA-API-KEY", token);
            var response = await Policy
                                    .HandleResult<IRestResponse<WhaleAlertResponse>>(r => !r.IsSuccessful)
                                    .Or<Exception>()
                                    .WaitAndRetryAsync(3,
                                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) / 2),
                                        (result, timeSpan, retryCount, context) =>
                                        {})
                                    .ExecuteAsync(() => restClient.ExecuteTaskAsync<WhaleAlertResponse>(request))
                                    .ConfigureAwait(false);

            if (response.IsSuccessful)
            {
               await InsertToDatabase(response.Data).ConfigureAwait(false);
            }
        }

        private async Task InsertToDatabase(WhaleAlertResponse resp)
        {
            if (resp.count == 0 || resp.result != "success") return;
            var data = PreProcessDataBeforeInsert(resp);

            if(latestWhaleAlert == null)
            {
                //Insert new record
                using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var sqlStatement = @"
                                        INSERT INTO [dbo].[WhaleAlerts] 
                                        ([Id]
                                        ,[Cursor]
                                        ,[CreatedDate]
                                        ,[JsonDetail]
                                        ,[LastUpdatedDate])
                                        VALUES 
                                        (@Id, @Cursor, @CreatedDate, @JsonDetail, @LastUpdatedDate)
                                        ";
                    await connection.ExecuteAsync(sqlStatement, data);
                }
            }
            else
            {
                //Update existing record
                using (var connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var sqlStatement = @"
                                        UPDATE [dbo].[WhaleAlerts] 
                                        SET 
                                         [Cursor] = @Cursor
                                        ,[CreatedDate] = @CreatedDate
                                        ,[JsonDetail] = @JsonDetail
                                        ,[LastUpdatedDate] =@LastUpdatedDate
                                        WHERE Id = @Id
                                        ";
                    await connection.ExecuteAsync(sqlStatement, data);
                }
            }
        }

        private WhaleAlert PreProcessDataBeforeInsert(WhaleAlertResponse resp)
        {
            var transactionJsonForDB = new List<WhaleAlertDBJson>();
            var symbolListToUpdate = Configuration.GetSection("HeaderKeys:SymbolList").Value.Split(',').ToList();
            var blockchainToLook = Configuration.GetSection("HeaderKeys:Blockchain").Value.Split(',').ToList();
            
            var concatenateToDB = latestWhaleAlert != null ? JsonSerializer.Deserialize<List<WhaleAlertDBJson>>(latestWhaleAlert.JsonDetail) : null;

                symbolListToUpdate.ForEach(sym =>
                {
                    var listOfTransactions = resp.transactions.Where(t => t.symbol == sym && blockchainToLook.Contains(t.blockchain)).Select(x => x);
                    var netInflow = listOfTransactions.Where(t => t.from.owner_type == "unknown" && t.to.owner_type == "exchange").Sum(t => t.amount_usd);
                    var netOutflow = listOfTransactions.Where(t => t.from.owner_type == "exchange" && t.to.owner_type == "unknown").Sum(t => t.amount_usd);
                    var netTotal = netOutflow - netInflow;
                    var existingCount = concatenateToDB?.FirstOrDefault(x => x.symbol == sym)?.net ?? 0;

                    transactionJsonForDB.Add(new WhaleAlertDBJson()
                    {
                        symbol = sym,
                        net = netTotal + existingCount
                    });
                });

            var detail = new WhaleAlert()
            {
                CreatedDate = latestWhaleAlert?.CreatedDate ?? DateTime.UtcNow,
                Cursor = resp.cursor,
                Id = latestWhaleAlert?.Id ?? Guid.NewGuid(),
                JsonDetail = JsonSerializer.Serialize(transactionJsonForDB),
                LastUpdatedDate = DateTime.UtcNow
            };
            return detail;
        }
    }
}
