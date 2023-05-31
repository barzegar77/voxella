using Api.Common;
using Api.Models;
using Api.Services.Interfaces;
using Api.ViewModels.Authentication;
using System.Text.RegularExpressions;

namespace Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(IUserRepository userRepository, 
            ITokenService tokenService, 
            IPasswordHasher passwordHasher, 
            IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterViewModel model)
        {
            var validEmail = IsValidEmail(model.Email);
            if (!validEmail)
            {
                return new AuthenticationResult(false, "Invalid email address. Please enter a valid email.");
            }

            var validUsername = IsValidUsername(model.Username);
            if (!validUsername)
            {
                return new AuthenticationResult(false, "Invalid username. The username can only contain English letters (uppercase or lowercase) and numbers.");
            }

            var passwordStrengthResult = VerifyPasswordStrength(model.Password);
            if (!passwordStrengthResult)
            {
                return new AuthenticationResult(false, "The password you provided is not strong enough. Please make sure your password meets the following requirements:\n- Minimum length of 6 characters\n- At least 3 unique characters\n- Contains at least one uppercase letter, one lowercase letter, one digit, and one special character.\nPlease try again with a stronger password.");
            }
            const int maxRegistrationAttempts = 10;
            const int minutesToWaitBetweenAttempts = 1;
            const int minutesToWaitForEmailConfirmation = 3;

            var existingUser = await _userRepository.GetUserByUsernameOrEmail(model.Username, model.Email);

            // Generate a unique verification code
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var verificationCode = code;


            // Send the verification email
            var emailSubject = "Email Confirmation";
            var emailMessage = $@"
            <html>
              <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                <h2 style='color: #333; font-size: 32px; margin-bottom: 20px; text-transform: uppercase;'>Dear {model.Username}!</h2>
                <p style='color: #555; font-size: 18px; line-height: 1.5;'>Please use the following verification code to confirm your email address:</p>
                <h3 style='color: #007bff; font-size: 28px; margin-top: 30px; background-color: #007bff; color: #fff; padding: 5px 10px; border-radius: 4px;' contenteditable='true'>{verificationCode}</h3>
                <p style='color: #888; font-size: 16px; margin-top: 40px; line-height: 1.5;'>Thank you for choosing Voxella, the intelligent video translation and dubbing app.</p>
                <p style='color: #888; font-size: 16px; margin-top: 40px; line-height: 1.5;'>With Voxella, you can easily translate and dub your videos into multiple languages, reaching a global audience.</p>
              </body>
            </html>";


            if (!existingUser.IsEmailConfirmed)
            {
                var lastEmailSentDateTime = existingUser.EmailConfirmationSentDateTime;
                var timeElapsedSinceLastEmail = DateTime.Now.Subtract(lastEmailSentDateTime.Value);

                // Check if the last email sent was less than minutesToWaitBetweenAttempts minutes ago
                if (timeElapsedSinceLastEmail.TotalMinutes < minutesToWaitBetweenAttempts)
                {
                    return new AuthenticationResult(false, "An email confirmation has already been sent. Please check your email.");
                }
                else
                {
                    // Update the user entity with the new verification code and sent datetime
                    existingUser.EmailConfirmationCode = verificationCode;
                    existingUser.EmailConfirmationSentDateTime = DateTime.Now;
                    existingUser.UpdatedDate = DateTime.Now;
                    existingUser.Password = _passwordHasher.HashPassword(model.Password);
                    existingUser.Email = model.Email;
                    existingUser.UserName = model.Username;

                    bool resultSendEmail = await _emailSender.SendEmailAsync(existingUser.Email, emailSubject, emailMessage);
                    if (!resultSendEmail)
                    {
                        return new AuthenticationResult(false, "Unfortunately, the email was not sent");
                    }

                    await _userRepository.UpdateUserAsync(existingUser); // Update the user entity

                    return new AuthenticationResult(true, "Email confirmation has been resent. Please check your email.");
                }
            }
            else if(existingUser.IsEmailConfirmed)
            {
                return new AuthenticationResult(false, "Username or email already exists.");
            }

            var hashedPassword = _passwordHasher.HashPassword(model.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = model.Username,
                Email = model.Email,
                Password = hashedPassword,
                CreatedDate = DateTime.Now,
                IsEmailConfirmed = false,
            };

         

            // Update the user entity with the verification code and sent datetime
            user.EmailConfirmationCode = verificationCode;
            user.EmailConfirmationSentDateTime = DateTime.Now;

            bool resSendEmail = await _emailSender.SendEmailAsync(user.Email, emailSubject, emailMessage);
            if (!resSendEmail)
            {
                return new AuthenticationResult(false, "Unfortunately, the email was not sent");
            }

            await _userRepository.AddUserAsync(user);
            return new AuthenticationResult(true, "User registered successfully.");
        }


        public bool IsValidEmail(string email)
        {
            const string emailRegexPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            var regex = new Regex(emailRegexPattern);
            return regex.IsMatch(email);
        }


        private bool IsValidUsername(string username)
        {
            const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            foreach (char c in username)
            {
                if (!ValidCharacters.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool VerifyPasswordStrength(string password)
        {
            const int RequiredLength = 6;
            const int RequiredUniqueChars = 3;
            const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";

            if (password.Length < RequiredLength)
            {
                return false;
            }

            if (password.Distinct().Count() < RequiredUniqueChars)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            if (!password.Any(c => ValidChars.Contains(c)))
            {
                return false;
            }

            return true;
        }


        public async Task<AuthenticationResult> EmailConfirmationAsync(EmailConfirmationViewModel model)
        {
            var user = await _userRepository.GetUserByEmail(model.Email);
            if(user == null)
            {
                return new AuthenticationResult(false, "Username or email already exists.");
            }
            else if(user.IsEmailConfirmed)
            {
                return new AuthenticationResult(false, "Username or email already exists.");
            }
            else if (user.EmailConfirmationCode == model.EmailConfirmationCode)
            {
                user.IsEmailConfirmed = true;
                await _userRepository.UpdateUserAsync(user);
                return new AuthenticationResult(true, "Your account has been successfully verified");
            }

            return new AuthenticationResult(false, "An error has occurred");
        }


        public async Task<AuthenticationResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null || !_passwordHasher.VerifyPassword(model.Password, user.Password))
            {
                return new AuthenticationResult(false, "Invalid username or password.");
            }

            var expiration = TimeSpan.FromMinutes(60); // Set the expiration time for the access token
            var accessToken = _tokenService.GenerateToken(user, expiration);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.UpdateUserAsync(user);

            return new AuthenticationResult(true, "Login successful.", accessToken, user.RefreshToken);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                return new AuthenticationResult(false, "Invalid refresh token.");
            }

            var expiration = TimeSpan.FromMinutes(60); // Set the expiration time for the access token
            var accessToken = _tokenService.GenerateToken(user, expiration);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.UpdateUserAsync(user);

            return new AuthenticationResult(true, "Token refreshed successfully.", accessToken, user.RefreshToken);
        }

        public async Task<AuthenticationResult> LogoutAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                return new AuthenticationResult(false, "Invalid refresh token.");
            }

            user.RefreshToken = null;
            await _userRepository.UpdateUserAsync(user);

            return new AuthenticationResult(true, "Logout successful.");
        }
    }
}