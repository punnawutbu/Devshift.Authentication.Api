namespace Devshift.Authentication.Api.Models
{
    public class AppSettings
    {
        public string VaultHost {get; set;}
        public string VaultToken {get; set;}
    }
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}