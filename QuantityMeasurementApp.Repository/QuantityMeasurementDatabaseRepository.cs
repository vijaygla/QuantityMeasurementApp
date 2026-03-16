using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Utilities;
using QuantityMeasurementApp.Exceptions;

namespace QuantityMeasurementApp.Repository
{
    /// <summary>
    /// Repository implementation that persists quantity measurement records into a SQL database.
    /// Implements <see cref="IQuantityMeasurementRepository"/> using ADO.NET.
    /// </summary>
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;
        private static ConnectionPool? _connectionPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityMeasurementDatabaseRepository"/> class.
        /// Loads the connection string from configuration and initializes the connection pool.
        /// </summary>
        public QuantityMeasurementDatabaseRepository()
        {
            _connectionString = DatabaseConfig.GetConnectionString();
            
            // Initialize the connection pool as a singleton-like instance for the repository.
            // This ensures efficient connection management across multiple operations.
            if (_connectionPool == null)
            {
                _connectionPool = new ConnectionPool(_connectionString);
            }
        }

        /// <summary>
        /// Saves a quantity measurement record to the database using ADO.NET and connection pooling.
        /// </summary>
        /// <param name="entity">The measurement entity containing the operands, operation, and result.</param>
        /// <exception cref="DatabaseException">Thrown when a database error occurs during the save operation.</exception>
        public void Save(QuantityMeasurementEntity entity)
        {
            SqlConnection? conn = null;
            try
            {
                // Obtain an open connection from the connection pool.
                conn = _connectionPool!.GetConnection();

                string query = @"INSERT INTO QuantityMeasurements
                                (Operand1Value, Operand1Unit,
                                 Operand2Value, Operand2Unit,
                                 Operation, Result, ErrorMessage)
                                VALUES
                                (@v1,@u1,@v2,@u2,@operation,@result,@error)";

                using SqlCommand cmd = new SqlCommand(query, conn);

                // Configure parameters to prevent SQL injection and handle potential null operands.
                cmd.Parameters.Add("@v1", SqlDbType.Float).Value =
                    entity.Operand1?.Value ?? (object)DBNull.Value;

                cmd.Parameters.Add("@u1", SqlDbType.NVarChar).Value =
                    entity.Operand1?.Unit ?? (object)DBNull.Value;

                cmd.Parameters.Add("@v2", SqlDbType.Float).Value =
                    entity.Operand2?.Value ?? (object)DBNull.Value;

                cmd.Parameters.Add("@u2", SqlDbType.NVarChar).Value =
                    entity.Operand2?.Unit ?? (object)DBNull.Value;

                cmd.Parameters.Add("@operation", SqlDbType.NVarChar).Value =
                    entity.Operation;

                cmd.Parameters.Add("@result", SqlDbType.NVarChar).Value =
                    entity.Result?.ToString() ?? (object)DBNull.Value;

                cmd.Parameters.Add("@error", SqlDbType.NVarChar).Value =
                    entity.ErrorMessage ?? (object)DBNull.Value;

                // Execute the insert command.
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Wrap SQL-specific exceptions into a custom domain exception.
                throw new DatabaseException("Database error while saving measurement", ex);
            }
            catch (Exception ex)
            {
                // Wrap unexpected exceptions into a custom domain exception.
                throw new DatabaseException("Unexpected error while saving measurement", ex);
            }
            finally
            {
                // Always release the connection back to the pool to prevent leaks.
                if (conn != null)
                {
                    _connectionPool!.ReleaseConnection(conn);
                }
            }
        }
    }
}
