using System;
using System.Threading;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Repositories;
using Devshift.ResponseMessage;
using Devshift.Security.Encryption;
using Devshift.Authentication.Api.Shared.Utils;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public class MemberFacade : IMemberFacade
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ISecurityEncryption _securityEncryption;

        public MemberFacade(IMemberRepository memberRepository, ISecurityEncryption securityEncryption)
        {
            _memberRepository = memberRepository;
            _securityEncryption = securityEncryption;
        }

        public async Task<ResponseMessage<RegisterResult>> Register(
            RegisterRequest req,
            string systemName,
            CancellationToken ct)
        {
            var resp = new ResponseMessage<RegisterResult>();
            try
            {
                if (req == null)
                {
                    resp.Message = Constants.Message.RegisterFailed;
                    return resp;
                }

                if (string.IsNullOrWhiteSpace(systemName))
                    {
                    resp.Message = Constants.Message.RegisterFailed;
                    return resp;
                }

                switch (systemName.ToLower())
                {
                    case "devshift":
                    case "nimitx-home":
                        resp.Result = await RegisterInternal(req, systemName, ct);
                        resp.Message = Constants.Message.RegisterSuccess;
                        return resp;
                    default:
                        resp.Message = Constants.Message.RegisterFailed;
                        return resp;
                }
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                return resp;
            }
        }

        private async Task<RegisterResult> RegisterInternal(
            RegisterRequest req,
            string systemName,
            CancellationToken ct)
        {
            var result = new RegisterResult();
            if (string.IsNullOrWhiteSpace(req.UserName))
                {
                    result.Message = Constants.Message.RegisterFailed;
                    return result;
                }

            req.UserName = req.UserName.Trim();
            req.SystemCode = systemName.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(req.AuthType))
                req.AuthType = "local";

            req.AuthType = req.AuthType.Trim().ToLower();

            if (req.AuthType == "local")
            {
                if (string.IsNullOrWhiteSpace(req.Password))
                {
                    result.Message = Constants.Message.RegisterFailed;
                    return result;
                }

                var passwordHash = _securityEncryption.BCryptComputeHash(req.Password.Trim());
                req.PasswordHash = passwordHash;
            }
            else
            {
                req.PasswordSalt = null;
                req.PasswordHash = null;
            }

            var exists = await _memberRepository.ExistsUserNameAsync(req.UserName, ct);
            if (exists)
            {
                result.Message = Constants.Message.UserAlreadyExists;
                return result;
            }

            result = await _memberRepository.RegisterAsync(req, ct);

            return result;
        }
    }
}