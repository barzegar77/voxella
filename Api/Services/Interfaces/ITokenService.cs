using Api.Models;

namespace Api.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, TimeSpan expiration);
        string GenerateRefreshToken();
    }
}