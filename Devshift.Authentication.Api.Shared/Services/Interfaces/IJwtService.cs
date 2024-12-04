using System.Security.Claims;

namespace Devshift.Authentication.Api.Shared.Services
{
    public interface IJwtService
    {
        string GenerateToken(string id, string systemName, string role, int lifeTime, string iss = "devshift");
        string GenerateToken(string systemName, Claim[] claims, int lifeTime, string iss = "devshift");
    }
}