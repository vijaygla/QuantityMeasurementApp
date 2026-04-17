using AuthService.Models;
using System.Linq;

namespace AuthService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public void AddUser(UserEntity user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public UserEntity? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
