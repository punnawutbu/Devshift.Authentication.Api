using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Services;
using Devshift.DateTimeProvider;
using Devshift.Jwt;

namespace Devshift.Authentication.Api.Shared.Facades
{
    public class DefaultLoginFacade : IDefaultLoginFacade
    {
        private readonly IJwtService _jwtService;
        private readonly IDateTimeProvider _endDateTime;
        public DefaultLoginFacade(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        public async Task<LoginResponse> Login(LoginRequest user, string systemName, int version = 1, string role = "user")
        {
            var resp = new LoginResponse();

            bool isPssswordCorrect = true;

            if (!isPssswordCorrect)
            {
                resp.Message = LoginMessage.UserOrPasswordIncorrect;
                return resp;
            }

            var profile = new Profiles();

            var token = _jwtService.GenerateToken(user.Username, systemName, role, 86400);
            var refreshToken = _jwtService.GenerateToken(user.Username, systemName, role, 7889231);

            resp.Token = token;
            resp.Profile = profile;
            resp.RefreshToken = refreshToken;

            // var isEnable = await _playerRepo.CheckIsEnable(user.Username);

            resp.Message = LoginMessage.LoginSuccess;
            return resp;
        }
        public RefreshTokenResponse RefreshToken(string token)
        {
            var resp = new RefreshTokenResponse();


            var now = _endDateTime.Now();

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
        private string _SubstringRole(string role)
        {
            var str = role.Split("\\");
            return str.Last();
        }
        private decimal _GetCredit(decimal marginFree, string userRole, int version)
        {
            if (version == 2 && userRole == "HAT")
            {
                return decimal.Round(marginFree, 4);
            }
            return decimal.Round(marginFree * 16.6667m, 4);
        }
        private DateTime _ConvertUnixToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTime);
        }
        #endregion
    }
}