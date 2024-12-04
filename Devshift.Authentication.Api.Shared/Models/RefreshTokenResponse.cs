namespace Devshift.Authentication.Api.Shared.Models
{
    public class RefreshTokenResponse
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
}