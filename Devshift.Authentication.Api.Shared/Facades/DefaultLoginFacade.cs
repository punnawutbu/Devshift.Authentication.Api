using System;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Repositories;
using Devshift.Authentication.Api.Shared.Services;
using Devshift.DateTimeProvider;
using Devshift.Jwt;
using Devshift.Security.Encryption;
using Devshift.Authentication.Api.Shared.Utils;
using Devshift.ResponseMessage;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public class DefaultLoginFacade : IDefaultLoginFacade
    {
        private readonly IJwtService _jwtService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMemberRepository _memberRepository;
        private readonly ISecurityEncryption _securityEncryption;
        private readonly JwtOptions _jwtOptions;
        public DefaultLoginFacade(
    IJwtService jwtService,
    IMemberRepository memberRepository,
    ISecurityEncryption securityEncryption,
    IDateTimeProvider dateTimeProvider,
    JwtOptions jwtOptions)
        {
            _jwtService = jwtService;
            _memberRepository = memberRepository;
            _securityEncryption = securityEncryption;
            _dateTimeProvider = dateTimeProvider;
            _jwtOptions = jwtOptions;
        }
        public async Task<ResponseMessage<Login>> Login(LoginRequest user, string systemName, int version = 1, string role = "user")
        {
            var resp = new ResponseMessage<Login>
            {
                Result = new Login(),
                Message = Constants.Message.UserOrPasswordIncorrect
            };
            try
            {
                string password = await _memberRepository.GetPassword(user.Username);

                bool isPssswordCorrect = _securityEncryption.BCryptVerify(user.Password, password);

                if (!isPssswordCorrect)
                {
                    return resp;
                }

                var userId = await _memberRepository.GetUserIdByUserName(user.Username);

                var userProfile = await _GetUserTokenProfile(userId);

                var claims = TokenUtils.GenerateClaims(userProfile, systemName, role);

                var token = JwtUtil.GenerateJwt(_jwtOptions.SecretKey, claims, 86400, systemName);
                var refreshToken = JwtUtil.GenerateJwt(_jwtOptions.SecretKey, claims, 7889231, systemName);

                resp.Result.Token = token;
                resp.Result.RefreshToken = refreshToken;

                resp.Message = Constants.Message.LoginSuccess;
                return resp;
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                return resp;
            }
        }
        public RefreshTokenResponse RefreshToken(string token)
        {
            var resp = new RefreshTokenResponse();


            var now = _dateTimeProvider.Now();

            var jwt = JwtUtil.ExtractJwt(token);

            var expire = _ConvertUnixToDateTime(int.Parse(jwt.Exp));

            if (now < expire)
            {
                resp.Token = _jwtService.GenerateToken(jwt.IdNumber, jwt.SystemName, jwt.Role, 86400);
                resp.RefreshToken = _jwtService.GenerateToken(jwt.IdNumber, jwt.SystemName, jwt.Role, 7889231);
            }

            return resp;
        }


        #region Private
        private DateTime _ConvertUnixToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTime);
        }

        private async Task<UserTokenProfile> _GetUserTokenProfile(Guid userId)
        {
            var user = await _memberRepository.GetUserById(userId);

            return user;
        }
        #endregion
    }
}