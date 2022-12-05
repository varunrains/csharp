using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace WhaleAlertFunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */3 * * * *")]TimerInfo myTimer, ILogger log)
        {
             var restClient = new RestClient("https://whalealert.azurewebsites.net/api/ExchangeInflowOutflow");
            //var restClient = new RestClient("https://localhost:44380/api/ExchangeInflowOutflow");
            var request = new RestRequest()
            {
                Method = Method.Get,
            };
            restClient.ExecuteAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
