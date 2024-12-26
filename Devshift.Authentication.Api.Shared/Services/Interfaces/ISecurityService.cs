namespace Devshift.Authentication.Api.Shared.Services
{
    public interface ISecurityService
    {
        string BCryptComputeHash(string pass);
        bool PasswordVerify(string pass, string hash);
    }
}