using Api.Services.Interfaces;
using Api.ViewModels.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _authenticationService.RegisterAsync(model);
            if (result.Success)
            {
                return new JsonResult(new { status = true, message = result.Message });
            }
            else
            {
                return new JsonResult(new { status = false, message = result.Message });
            }
        }


        [HttpPost("email-confirmation")]
        public async Task<IActionResult> EmailConfirmation(EmailConfirmationViewModel model)
        {
            var result = await _authenticationService.EmailConfirmationAsync(model);
            if (result.Success)
            {
                return new JsonResult(new { status = true, message = result.Message });
            }
            else
            {
                return new JsonResult(new { status = false, message = result.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _authenticationService.LoginAsync(model);
            if (result.Success)
            {
                return new JsonResult(new { status = true, message = result.Message, accessToken = result.Token, refreshToken = result.RefreshToken });
            }
            else
            {
                return new JsonResult(new { status = false, message = result.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string RefreshToken)
        {
            var result = await _authenticationService.RefreshTokenAsync(RefreshToken);
            if (result.Success)
            {
                return new JsonResult(new { status = true, message = result.Message, accessToken = result.Token, refreshToken = result.RefreshToken });
            }
            else
            {
                return new JsonResult(new { status = false, message = result.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string RefreshToken)
        {
            var result = await _authenticationService.LogoutAsync(RefreshToken);
            if (result.Success)
            {
                return new JsonResult(new { status = true, message = result.Message, accessToken = result.Token, refreshToken = result.RefreshToken });
            }
            else
            {
                return new JsonResult(new { status = false, message = result.Message });
            }
        }
    }
}
