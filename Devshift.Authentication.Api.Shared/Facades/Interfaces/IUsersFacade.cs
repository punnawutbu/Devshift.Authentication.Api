using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.ResponseMessage;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public interface IUsersFacade
    {
        Task<ResponseMessage<RegisterActivate>> Register(Register req);
    }
}