using Devshift.Security.Encryption;


namespace Devshift.Authentication.Api.Shared.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityEncryption _securityEncryption;
        public SecurityService(ISecurityEncryption securityEncryption)
        {
            _securityEncryption = securityEncryption;
        }
        public string BCryptComputeHash(string pass)
        {
            return _securityEncryption.BCryptComputeHash(pass);
        }
        public bool PasswordVerify(string pass, string hash)
        {
            return _securityEncryption.BCryptVerify(pass, hash);
        }

    }
}
