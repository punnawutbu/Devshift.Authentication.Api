
namespace Devshift.Authentication.Api.Shared.Models
{
    public class Register
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegisterActivate
    {
        public string Id { get; set; }
        public string UserName { get; set; }

    }
}