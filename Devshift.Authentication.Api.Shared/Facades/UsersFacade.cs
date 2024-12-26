using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Repositories;
using Devshift.Authentication.Api.Shared.Services;
using Devshift.ResponseMessage;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public class UsersFacade : IUsersFacade
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ISecurityService _securityService;
        private readonly IJwtService _jwtService;
        public UsersFacade(IMemberRepository memberRepository, ISecurityService securityService, IJwtService jwtService)
        {
            _memberRepository = memberRepository;
            _securityService = securityService;
            _jwtService = jwtService;
        }
        public async Task<ResponseMessage<RegisterActivate>> Register(Register req)
        {
            var resp = new ResponseMessage<RegisterActivate>
            {
                Result = null,
                Message = Message.Fail
            };

            var user = await _memberRepository.FindUser(req.UserName);
            if (user == null)
            {
                req.Password = _securityService.BCryptComputeHash(req.Password);
                resp.Result = await _memberRepository.CreateUser(req);
                resp.Message = Message.Created;
                return resp;
            }
            return resp;
        }
    }
}