using System;

namespace Devshift.Authentication.Api.Shared.Models;

public class RegisterResult
{
    public Guid UserId { get; set; }
    public Guid UserAuthenId { get; set; }
    public string UserName { get; set; } = null!;
    public string SystemCode { get; set; } = null!;
    public string AuthType { get; set; } = null!;
    public string Message { get; set; }
}