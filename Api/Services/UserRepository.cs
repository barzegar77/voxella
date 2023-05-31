using Api.Models;
using Api.Models.Data;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByUsernameOrEmail(string username, string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

    }
}