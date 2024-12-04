namespace Devshift.Authentication.Api.Shared.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class LoginResponse
    {
        public Profiles Profile { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
    }
    public class CheckPasswordRequest
    {
        public string Login { get; set; }
        public string Type { get; set; }
        public string Password { get; set; }
    }
    public class Profiles
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool HasPin { get; set; }
        public bool? IsEnable { get; set; }
    }

}