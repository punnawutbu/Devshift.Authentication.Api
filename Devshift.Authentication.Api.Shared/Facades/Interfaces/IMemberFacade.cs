using System.Threading;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.ResponseMessage;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public interface IMemberFacade
    {
        Task<ResponseMessage<RegisterResult>> Register(RegisterRequest req, string systemName, CancellationToken ct);
    }
}