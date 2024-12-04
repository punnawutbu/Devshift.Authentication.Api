using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Devshift.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Devshift.Authentication.Api.Shared.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        public JwtService(string secretKey)
        {
            _secretKey = secretKey;
        }
        public string GenerateToken(string id, string systemName, string role, int lifeTime, string iss = "devshift")
        {
            var startDate = DateTime.UtcNow;

            var endDate = DateTime.UtcNow.AddSeconds(lifeTime);


            var Claims = new Claim[]
            {
            new Claim("idNumber", id),
            new Claim("systemName", systemName),
            new Claim("role", role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(iss, null, Claims, startDate, endDate, credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GenerateToken(string systemName, Claim[] claims, int lifeTime, string iss = "devshift")
        {
            switch (systemName.ToLower())
            {
                case "devshift":
                    return JwtUtil.GenerateJwt(_secretKey, claims, lifeTime);
                default:
                    return null;
            }
        }

    }
}