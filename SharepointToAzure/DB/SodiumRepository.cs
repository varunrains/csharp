
using SharepointToAzure.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SharepointToAzure.DB
{
    public class SodiumRepository
    {
        public string SQL_Query = $"SELECT FileId, Type,Title,Url,CreatedOn,RepositoryType,LocationUrl,FolderUrl,SegmentationType FROM Sodium.Files F JOIN Sodium.RepositoryConfiguration RC ON F.RepositoryConfigurationId = RC.RepositoryConfigurationId  WHERE CreatedOn BETWEEN (@StartDate) AND(@EndDate) AND RepositoryType IN('Sharepoint','SharepointOnline') ORDER BY CreatedOn";

        public SodiumRepository()
        {
        }

        public async Task<List<FilesWithRepositoryInformation>> GetFilesWithRepositoryConfiguration()
        {
           
            var dbConnectionString = ConfigurationManager.AppSettings["DBConnectionString"];
          //  var dbConnectionString = ConfigurationManager.ConnectionStrings["CarbonQA"].ToString();
            var startDate = ConfigurationManager.AppSettings["StartDate"];
            var endDate = ConfigurationManager.AppSettings["EndDate"];
            List<FilesWithRepositoryInformation> files = new List<FilesWithRepositoryInformation>();
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                SqlCommand cmd = new SqlCommand(SQL_Query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate;
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
