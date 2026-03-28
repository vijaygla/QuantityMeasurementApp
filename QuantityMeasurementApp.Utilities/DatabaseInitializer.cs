using System;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace QuantityMeasurementApp.Utilities
{
    /// <summary>
    /// Handles database and schema initialization at application startup.
    /// Why: To ensure the SQL database and required tables exist without manual user setup.
    /// How: Connects to the master database to create the DB, then connects to the app DB to create the table schema.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initializes the database and tables based on the configured connection string.
        /// Why: To provide a "run-out-of-the-box" experience for the user.
        /// </summary>
        public static void Initialize()
        {
            string fullConnectionString = DatabaseConfig.GetConnectionString();
            
            // Extract the database name from the connection string using a regex.
            string dbName = "QuantityMeasurementDB";
            var match = Regex.Match(fullConnectionString, @"Database=([^;]+)", RegexOptions.IgnoreCase);
            if (match.Success) dbName = match.Groups[1].Value.Trim();

            // Create a connection string for the 'master' database to perform DB creation.
            string masterConnectionString = Regex.Replace(fullConnectionString, @"Database=[^;]+", "Database=master", RegexOptions.IgnoreCase);

            int retryCount = 0;
            const int maxRetries = 10;
            
            while (retryCount < maxRetries)
            {
                try
                {
                    Console.WriteLine($"--- [DB INITIALIZATION] Checking Database existence (Attempt {retryCount + 1})...");
                    using (var masterConn = new SqlConnection(masterConnectionString))
                    {
                        masterConn.Open();
                        
                        // Check if DB exists.
                        string checkDbQuery = $"SELECT database_id FROM sys.databases WHERE name = '{dbName}'";
                        using (var cmd = new SqlCommand(checkDbQuery, masterConn))
                        {
                            var result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                Console.WriteLine($"--- [DB INITIALIZATION] Creating Database '{dbName}'...");
                                string createDbQuery = $"CREATE DATABASE [{dbName}]";
                                using (var createCmd = new SqlCommand(createDbQuery, masterConn))
                                {
                                    createCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // Now connect to the actual database to create the table.
                    using (var appConn = new SqlConnection(fullConnectionString))
                    {
                        appConn.Open();
                        
                        Console.WriteLine("--- [DB INITIALIZATION] Ensuring schema exists...");
                        string createTableQuery = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'QuantityMeasurements')
                            BEGIN
                                CREATE TABLE QuantityMeasurements
                                (
                                    Id INT IDENTITY(1,1) PRIMARY KEY,
                                    Operand1Value FLOAT,
                                    Operand1Unit NVARCHAR(50),
                                    Operand2Value FLOAT NULL,
                                    Operand2Unit NVARCHAR(50) NULL,
                                    Operation NVARCHAR(50),
                                    Result NVARCHAR(100) NULL,
                                    ErrorMessage NVARCHAR(MAX) NULL,
                                    CreatedAt DATETIME DEFAULT GETDATE()
                                );
                            END
                            ELSE
                            BEGIN
                                -- Ensure ErrorMessage column exists in case an older version was used.
                                IF NOT EXISTS (SELECT * FROM sys.columns 
                                               WHERE object_id = OBJECT_ID('QuantityMeasurements') 
                                               AND name = 'ErrorMessage')
                                BEGIN
                                    ALTER TABLE QuantityMeasurements ADD ErrorMessage NVARCHAR(MAX) NULL;
                                END
                            END
                            
                            -- Ensure Users table exists
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                            BEGIN
                                CREATE TABLE Users
                                (
                                    Id INT IDENTITY(1,1) PRIMARY KEY,
                                    Name NVARCHAR(100) NOT NULL,
                                    Email NVARCHAR(100) NOT NULL UNIQUE,
                                    PasswordHash NVARCHAR(255) NOT NULL,
                                    Salt NVARCHAR(255) NOT NULL,
                                    CreatedAt DATETIME DEFAULT GETDATE()
                                );
                            END";

                        using (var cmd = new SqlCommand(createTableQuery, appConn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("--- [DB INITIALIZATION] Database is ready.");
                    return; // Success!
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"--- [DB INITIALIZATION WARNING] Could not auto-initialize database after {maxRetries} attempts: {ex.Message}");
                        Console.WriteLine("--- [DB INITIALIZATION WARNING] Please ensure SQL Server is running and connection string is correct.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"--- [DB INITIALIZATION] SQL Server not ready, retrying in 5s... ({ex.Message})");
                        System.Threading.Thread.Sleep(5000);
                    }
                }
            }
        }
    }
}
