namespace Api.ViewModels.Authentication
{
    public class AuthenticationResult
    {
        public AuthenticationResult(bool success, string message, string token = "", string refreshToken = "")
        {
            Success = success;
            Message = message;
            Token = token;
            RefreshToken = refreshToken;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}