using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public interface IDefaultLoginFacade
    {
        Task<LoginResponse> Login(LoginRequest user, string systemName, int version = 1, string role = "user");
    }
}