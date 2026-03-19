namespace Devshift.Authentication.Api.Shared.Models;

public class RegisterRequest
{
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string? PhoneNumber { get; set; }

    public string SystemCode { get; set; } = null!;
    public string AuthType { get; set; } = "local";

    public string? Password { get; set; }

    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }

    public string? ExternalUserId { get; set; }
    public string? CreatedBy { get; set; }
}