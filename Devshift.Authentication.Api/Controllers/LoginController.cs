using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Attributes;
using Devshift.Authentication.Api.Shared.Facades;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Devshift.Authentication.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IAuthenFacade _facade;
        public LoginController(ILogger<LoginController> logger, IAuthenFacade facade)
        {
            _logger = logger;
            _facade = facade;
        }
        [HttpPost]
        public async Task<IActionResult> Login(
    [FromBody] LoginRequest user,
    [FromHeader(Name = "system-name")] string systemName)
        {
            _logger.LogInformation("V.1 Login User {Username}", user.Username);
            var jwt = AuthenticationAttribute.GetJwt();

            var resp = await _facade.Login(user, systemName);

            if (resp.Message == Constants.Message.LoginSuccess)
            {
                return Ok(resp);
            }

            return Unauthorized(resp);
        }
    }
}