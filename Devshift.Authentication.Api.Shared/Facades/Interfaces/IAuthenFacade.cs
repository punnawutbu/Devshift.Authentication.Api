using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public interface IAuthenFacade
    {
        Task<LoginResponse> Login(LoginRequest user, string systemName, int version = 1);
    }
}