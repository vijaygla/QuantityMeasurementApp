using Microsoft.Extensions.Configuration;
using System.IO;

namespace QuantityMeasurementApp.Utilities
{
    /// <summary>
    /// Configuration helper for the database layer.
    /// Manages retrieval of settings from the appsettings.json configuration file.
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Retrieves the connection string named "DefaultConnection" from the configuration.
        /// Why: To abstract the configuration source (JSON) from the database layer.
        /// How: Loads appsettings.json via ConfigurationBuilder and fetches the specific key.
        /// </summary>
        /// <returns>The configured SQL connection string.</returns>
        /// <exception cref="System.Exception">Thrown when the connection string is missing.</exception>
        public static string GetConnectionString()
        {
            // Set base path to current directory to ensure file is found in both build and run scenarios.
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Extract connection string by name.
            string? connStr = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new System.Exception("Critical Error: 'DefaultConnection' string not found in appsettings.json.");
            }

            return connStr;
        }
    }
}
