using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Utilities;
using Microsoft.Data.SqlClient;
namespace QuantityMeasurementApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly JwtService _jwt;

        public AuthService(IUserRepository repo, JwtService jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        // 🔐 Register new user
        public string Register(string name, string email, string password)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Email and Password are required");

            // Check if user already exists (first level validation)
            var existingUser = _repo.GetUserByEmail(email);
            if (existingUser != null)
                throw new Exception("User already exists with this email");

            // Create secure password (salt + hash)
            var salt = PasswordHasher.GenerateSalt();
            var hash = PasswordHasher.HashPassword(password, salt);

            var user = new UserEntity
            {
                Name = name,
                Email = email,
                PasswordHash = hash,
                Salt = salt
            };

            // Save user (second level protection using DB constraint)
            try
            {
                _repo.AddUser(user);
            }
            catch (SqlException ex) when (ex.Number == 2627) // duplicate key
            {
                throw new Exception("Email already exists");
            }
            catch (Exception)
            {
                throw new Exception("Error while registering user");
            }

            return "User registered successfully";
        }

        // 🔐 Login existing user
        public string Login(string email, string password)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Email and Password are required");

            // Fetch user from DB
            var user = _repo.GetUserByEmail(email);
            if (user == null)
                throw new Exception("User not found");

            // Verify password
            var hash = PasswordHasher.HashPassword(password, user.Salt);
            if (hash != user.PasswordHash)
                throw new Exception("Invalid password");

            // Generate JWT token
            return _jwt.GenerateToken(email);
        }
    }
}
