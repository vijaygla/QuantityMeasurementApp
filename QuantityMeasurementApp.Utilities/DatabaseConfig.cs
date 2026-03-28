using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace QuantityMeasurementApp.Utilities
{
    /// <summary>
    /// Manages the database connection string configuration.
    /// </summary>
    public static class DatabaseConfig
    {
        private static string? _connectionString;

        /// <summary>
        /// Initializes the connection string from the provided configuration.
        /// </summary>
        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new Exception("DefaultConnection not found in configuration.");
            }
        }

        /// <summary>
        /// Gets the current database connection string.
        /// </summary>
        public static string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                _connectionString = config.GetConnectionString("DefaultConnection") 
                    ?? "Server=localhost,1433;Database=QuantityMeasurementDB;User Id=sa;Password=Your_Password123;TrustServerCertificate=True;";
            }

            return _connectionString;
        }
    }
}
