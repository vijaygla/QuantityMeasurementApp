using System;
using System.Collections.Concurrent;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurementApp.Utilities
{
    /// <summary>
    /// Custom connection pool for managing SQL database connections.
    /// Provides efficient reuse of <see cref="SqlConnection"/> objects to reduce overhead of establishing new connections.
    /// Designed for real-time applications where high throughput and low latency are critical.
    /// </summary>
    public class ConnectionPool
    {
        private readonly string connectionString;
        private readonly ConcurrentBag<SqlConnection> connections = new();

        private readonly int maxPoolSize;
        private int currentConnections = 0;
        private readonly object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionPool"/> class.
        /// </summary>
        /// <param name="connectionString">The SQL connection string to use.</param>
        /// <param name="maxPoolSize">The maximum number of connections to allow.</param>
        public ConnectionPool(string connectionString, int maxPoolSize = 10)
        {
            this.connectionString = connectionString;
            this.maxPoolSize = maxPoolSize;
        }

        /// <summary>
        /// Retrieves an open <see cref="SqlConnection"/> from the pool or creates a new one if necessary and possible.
        /// </summary>
        /// <returns>An open database connection.</returns>
        /// <exception cref="Exception">Thrown when the connection pool has reached its maximum size and no connections are available.</exception>
        public SqlConnection GetConnection()
        {
            // Try to retrieve a connection from the pool.
            if (connections.TryTake(out SqlConnection? connection) && connection != null)
            {
                // Ensure the connection is open before returning it.
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return connection;
            }

            // Create a new connection if we haven't reached the maximum pool size.
            lock (lockObject)
            {
                if (currentConnections < maxPoolSize)
                {
                    var newConnection = new SqlConnection(connectionString);
                    newConnection.Open();

                    currentConnections++;

                    return newConnection;
                }
            }

            // Exhausted pool state.
            throw new Exception("Connection Pool exhausted. All available connections are currently in use.");
        }

        /// <summary>
        /// Returns a connection to the pool for reuse.
        /// </summary>
        /// <param name="connection">The connection to return.</param>
        public void ReleaseConnection(SqlConnection connection)
        {
            if (connection != null)
            {
                // Add the connection back to the thread-safe collection.
                connections.Add(connection);
            }
        }

        /// <summary>
        /// Provides statistics about the current state of the connection pool.
        /// </summary>
        /// <returns>A string summarizing pool usage.</returns>
        public string GetPoolStatistics()
        {
            return $"Active Connections: {currentConnections}, Available in Pool: {connections.Count}";
        }

        /// <summary>
        /// Closes and disposes all connections in the pool.
        /// </summary>
        public void CloseAll()
        {
            while (connections.TryTake(out SqlConnection? connection) && connection != null)
            {
                connection.Close();
                connection.Dispose();
            }

            lock (lockObject)
            {
                currentConnections = 0;
            }
        }
    }
}
