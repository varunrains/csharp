using LinkedInRecruiterScraper.Interfaces;
using LinkedInRecruiterScraper.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInRecruiterScraper.Data
{
    public class SQLLiteRepository :IRepository
    {

        private readonly string _connectionString;
        private readonly string _dbPath;

        public SQLLiteRepository(string dbPath)
        {
            _dbPath = dbPath;
            _connectionString = $"Data Source={dbPath};Version=3;";
            EnsureDatabaseAndTableExists();
        }

        private void EnsureDatabaseAndTableExists()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Jobs (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            JobDescription TEXT NOT NULL,
                            JobUrl TEXT NOT NULL,
                            Title TEXT NOT NULL,
                            Company TEXT NOT NULL,
                            Location TEXT,
                            HiringManagerLink TEXT,
                            CreateDate TEXT DEFAULT CURRENT_TIMESTAMP
                        );

                            CREATE TABLE IF NOT EXISTS HiringManagers (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            HiringManagerLink TEXT,
                            ConnectionDegree TEXT,
                            CreateDate TEXT DEFAULT CURRENT_TIMESTAMP
                        );
                    ";

                    command.ExecuteNonQuery();
                }
            }
        }

        public int InsertJob(JobRecord job)
        {
            int newId = 0;

            if (CheckIfJobExists(job.JobUrl))
            {
                return 0;
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Begin transaction for better performance
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = @"
                                INSERT INTO Jobs (Title, Company, Location, JobDescription, JobUrl, HiringManagerLink)
                                VALUES (@Title, @Company, @Location, @JobDescription, @JobUrl, @HiringManagerLink);
                                SELECT last_insert_rowid();
                            ";

                            // Use parameters to prevent SQL injection
                            command.Parameters.AddWithValue("@Title", job.Title);
                            command.Parameters.AddWithValue("@Company", job.Company);
                            command.Parameters.AddWithValue("@Location", job.Location ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@JobDescription", job.JobDescription ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@JobUrl", job.JobUrl ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@HiringManagerLink", job.HiringManagerLink);

                            // Get the newly generated ID
                            newId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error inserting record: {ex.Message}");
                        throw;
                    }
                }
            }

            return newId;
        }

        public int InsertHiringManager(HiringManager hiringManager)
        {
            int newId = 0;

            if (string.IsNullOrWhiteSpace(hiringManager.HiringManagerLink) || CheckIfHiringManagerExists(hiringManager.HiringManagerLink))
            {
                return 0;
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Begin transaction for better performance
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SQLiteCommand(connection))
                        {
                            command.CommandText = @"
                                INSERT INTO HiringManagers (HiringManagerLink, ConnectionDegree)
                                VALUES (@HiringManagerLink, @ConnectionDegree);
                                SELECT last_insert_rowid();
                            ";

                            // Use parameters to prevent SQL injection
                           
                            command.Parameters.AddWithValue("@HiringManagerLink", hiringManager.HiringManagerLink);
                            command.Parameters.AddWithValue("@ConnectionDegree", hiringManager.ConnectionDegree);

                            // Get the newly generated ID
                            newId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error inserting record: {ex.Message}");
                        throw;
                    }
                }
            }

            return newId;
        }


        public bool CheckIfHiringManagerExists(string hiringManagerUrl)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(1) FROM HiringManagers WHERE HiringManagerLink = @HiringManagerLink";
                    command.Parameters.AddWithValue("@HiringManagerLink", hiringManagerUrl);

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        public bool CheckIfJobExists(string jobUrl)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(1) FROM Jobs WHERE JobUrl = @JobUrl";
                    command.Parameters.AddWithValue("@JobUrl", jobUrl);

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
