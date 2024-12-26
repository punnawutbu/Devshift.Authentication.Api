using Devshift.Dapper.Models;

namespace Devshift.Authentication.Api.Models
{
    public class AppSettings
    {
        public string VaultHost {get; set;}
        public string VaultToken {get; set;}
    }
    public class Credential
    {
        public string MemberConnectionString {get; set;}
        public Certificate Certificate { get; set; }
        public bool SslMode { get; set; }
        public string HashKey { get; set; }
    }
}