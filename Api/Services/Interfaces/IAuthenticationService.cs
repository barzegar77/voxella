using Api.ViewModels.Authentication;

namespace Api.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> RegisterAsync(RegisterViewModel model);
        Task<AuthenticationResult> EmailConfirmationAsync(EmailConfirmationViewModel model);
        Task<AuthenticationResult> LoginAsync(LoginViewModel model);
        Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
        Task<AuthenticationResult> LogoutAsync(string refreshToken);
    }
}