using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SalesAPI.Tests.Infrastructure
{
    public class PostgresDbFixture : IDisposable
    {
        private readonly string _connectionString;

        public PostgresDbFixture()
        {
            // Set your PostgreSQL server connection string here
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure it looks in the correct directory
            .AddJsonFile("appsettings.json") // Load appsettings.json
            .Build();

            _connectionString = configuration.GetConnectionString("TestDatabase");

            // Ensure the connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'TestDatabase' not found in appsettings.json.");
            };
        }

        public string GetConnectionString(string dbName)
        {
            // Return a unique connection string for each test run.
            return $"{_connectionString};Database={dbName}";
        }

        public void Dispose()
        {
           
        }

        public void SetupTestDatabase(string dbName)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            // Create a unique test database
            var command = new NpgsqlCommand($"CREATE DATABASE {dbName}", conn);
            command.ExecuteNonQuery();
        }

        public void CleanupTestDatabase(string dbName)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            // Step 1: Check for active connections to the database
            var checkConnectionsCmd = new NpgsqlCommand(@$"
                    SELECT COUNT(*)
                    FROM pg_stat_activity
                    WHERE datname = '{dbName}' AND pid <> pg_backend_pid();
                ", conn);

            var activeConnections = (long)checkConnectionsCmd.ExecuteScalar();

            if (activeConnections > 1)
            {
                Console.WriteLine($"Database '{dbName}' has {activeConnections} active connection(s). Cannot drop the database.");
                return;  // Skip dropping the database or handle accordingly
            }


            // Step 2: Terminate any active connections to the test database
            using var terminateCmd = new NpgsqlCommand(@$"
                            SELECT pg_terminate_backend(pid)
                            FROM pg_stat_activity
                            WHERE datname = '{dbName}' AND pid <> pg_backend_pid();
                        ", conn);

            terminateCmd.ExecuteNonQuery();

            // Step 3: Drop the test database
            using var dropCmd = new NpgsqlCommand($"DROP DATABASE IF EXISTS \"{dbName}\";", conn);
            dropCmd.ExecuteNonQuery();

        }


    }

}
