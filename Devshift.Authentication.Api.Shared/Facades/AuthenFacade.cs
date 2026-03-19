using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Utils;
using Devshift.ResponseMessage;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public class AuthenFacade : IAuthenFacade
    {
        private readonly IDefaultLoginFacade _defaultLoginFacade;
        // private readonly IApplicationRepository _applicationRepo;
        public AuthenFacade(IDefaultLoginFacade defaultLoginFacade)
        {
            _defaultLoginFacade = defaultLoginFacade;
            // _applicationRepo = applicationRepo;
        }
        public async Task<ResponseMessage<Login>> Login(LoginRequest user, string systemName, int version = 1)
        {
            var resp = new ResponseMessage<Login>();
            resp.Message = Constants.Message.LoginFailed;
            switch (systemName)
            {
                case "devshift":
                    return await _defaultLoginFacade.Login(user, systemName, version);
                default:
                    return resp;
            }
        }
        // public ResponseMessage<Login> RefreshToken(string token, string systemName)
        // {
        //     return new RefreshTokenResponse();
        // }
        public async Task<bool> Logout(string token, string systemName)
        {
            var resp = 1;
            // var resp = await _applicationRepo.InsertBlackListToken(token, systemName);


            return resp > 0;
        }

    }
}