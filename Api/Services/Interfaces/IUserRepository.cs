using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsernameOrEmail(string username, string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByRefreshToken(string refreshToken);
    }
}