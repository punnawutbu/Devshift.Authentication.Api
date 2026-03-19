using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Devshift.Authentication.Api.Shared.Utils
{
    public static class TokenUtils
    {
        public static Claim[] GenerateClaims<T>(T userProfile, string systemName, string role)
        {
            switch (systemName)
            {
                case "devshift":
                    return _GenerateClaimsForDevshift(userProfile, systemName);
                case "nimitx":
                    return _GenerateClaimsForNimitx(userProfile, systemName);
                default:
                    return new Claim[0];
            }
        }
        private static Claim[] _GenerateClaimsForDevshift<T>(T userProfile, string systemName)
        {
            if (userProfile == null)
                throw new ArgumentNullException(nameof(userProfile));

            var type = userProfile.GetType();

            var roles = type.GetProperty("Roles")?.GetValue(userProfile) as IEnumerable<string> ?? Enumerable.Empty<string>();
            var permissions = type.GetProperty("Permissions")?.GetValue(userProfile) as IEnumerable<string> ?? Enumerable.Empty<string>();
            var idNumber = type.GetProperty("IdNumber")?.GetValue(userProfile)?.ToString() ?? "";

            var claims = new List<Claim>
            {
                new Claim("system", systemName),
                new Claim("idNumber", idNumber),
                new Claim("permissions", string.Join(",", permissions))
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            return claims.ToArray();
        }
        private static Claim[] _GenerateClaimsForNimitx<T>(T userProfile, string systemName)
        {
            var claims = new Claim[]
            {
                new Claim("system", systemName),
                new Claim(ClaimTypes.Role, userProfile.GetType().GetProperty("Role").GetValue(userProfile).ToString())
            };

            return claims;
        }
    }
}