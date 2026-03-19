using System;
using Devshift.Jwt;
using Devshift.Jwt.Models;

namespace Devshift.Authentication.Api.Shared.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthenticationAttribute : Attribute
{
    private static JwtBase Token;

    public JwtBase Authorize(string token, string systemName)
    {
        var extractJwt = JwtUtil.ExtractJwt(token);
        Token = extractJwt;

        return extractJwt;
    }

    public static JwtBase GetJwt() => Token;
}