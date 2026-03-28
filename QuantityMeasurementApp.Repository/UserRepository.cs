using Microsoft.Data.SqlClient;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Utilities;

namespace QuantityMeasurementApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = DatabaseConfig.GetConnectionString();
        }

        // 🗄️ Add new user to DB
        public void AddUser(UserEntity user)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var query = @"INSERT INTO Users (Name, Email, PasswordHash, Salt) 
                          VALUES (@Name, @Email, @PasswordHash, @Salt)";

            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Name", user.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@Email", user.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? string.Empty);
            cmd.Parameters.AddWithValue("@Salt", user.Salt ?? string.Empty);

            cmd.ExecuteNonQuery();
        }

        // 🔍 Get user by email (used for login + duplicate check)
        public UserEntity? GetUserByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var query = "SELECT Id, Name, Email, PasswordHash, Salt FROM Users WHERE Email = @Email";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();

            // If user found → map data to object
            if (reader.Read())
            {
                return new UserEntity
                {
                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                    Name = reader["Name"]?.ToString() ?? string.Empty,
                    Email = reader["Email"]?.ToString() ?? string.Empty,
                    PasswordHash = reader["PasswordHash"]?.ToString() ?? string.Empty,
                    Salt = reader["Salt"]?.ToString() ?? string.Empty
                };
            }

            // If no user found → return null
            return null;
        }
    }
}
