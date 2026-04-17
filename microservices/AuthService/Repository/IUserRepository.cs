using AuthService.Models;

namespace AuthService.Repository
{
    public interface IUserRepository
    {
        void AddUser(UserEntity user);
        UserEntity? GetUserByEmail(string email);
    }
}
