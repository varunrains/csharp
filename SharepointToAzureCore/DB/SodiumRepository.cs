using Microsoft.Extensions.Options;
using SharepointToAzureCore.Configuration;
using SharepointToAzureCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzureCore.DB
{
    public class SodiumRepository
    {
        public string SQL_Query = $"SELECT FileId, Type,Title,Url,CreatedOn,RepositoryType,LocationUrl,FolderUrl,SegmentationType FROM Sodium.Files F JOIN Sodium.RepositoryConfiguration RC ON F.RepositoryConfigurationId = RC.RepositoryConfigurationId  WHERE CreatedOn BETWEEN (@StartDate) AND(@EndDate) AND RepositoryType IN('Sharepoint','SharepointOnline') ORDER BY CreatedOn";

        public IOptions<BptConfiguration> _bptConfiguration;
        public SodiumRepository(IOptions<BptConfiguration> bptConfiguration)
        {
            _bptConfiguration = bptConfiguration;
        }

        public async Task<List<FilesWithRepositoryInformation>> GetFilesWithRepositoryConfiguration()
        {
            List<FilesWithRepositoryInformation> files = new List<FilesWithRepositoryInformation>();
            using (SqlConnection con = new SqlConnection(_bptConfiguration.Value.DBConnectionString))
            {
                SqlCommand cmd = new SqlCommand(SQL_Query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _bptConfiguration.Value.StartDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _bptConfiguration.Value.EndDate;
                con.Open();
                SqlDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (rdr.Read())
                {
                    FilesWithRepositoryInformation file = new FilesWithRepositoryInformation();
                    file.FileId = Guid.Parse(rdr["FileId"].ToString());
                    file.Type = rdr["Type"].ToString();
                    file.Title = rdr["Title"].ToString();
                    file.Url = rdr["Url"].ToString();
                    file.CreatedOn = Convert.ToDateTime(rdr["CreatedOn"].ToString());
                    file.RepositoryType = rdr["RepositoryType"].ToString();
                    file.LocationUrl = rdr["LocationUrl"].ToString();
                    file.FolderUrl = rdr["FolderUrl"].ToString();
                    file.SegmentationType = rdr["SegmentationType"].ToString();
                    files.Add(file);
                }
                con.Close();
            }
            return files;
        }
    }
}
