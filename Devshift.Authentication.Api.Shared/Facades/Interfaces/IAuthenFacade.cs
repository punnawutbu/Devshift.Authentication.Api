using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.ResponseMessage;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public interface IAuthenFacade
    {
        Task<ResponseMessage<Login>> Login(LoginRequest user, string systemName, int version = 1);
    }
}