using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Facades;
using Devshift.Authentication.Api.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

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
        public async Task<IActionResult> Login(LoginRequest user)
        {
            _logger.LogInformation($"V.1 Login User {user.Username}");
            Request.Headers.TryGetValue("system-name", out StringValues systemName);

            var resp = await _facade.Login(user, systemName[0]);

            if (!string.IsNullOrEmpty(resp.Token))
            {
                return Ok(resp);
            }
            return Unauthorized(resp);
        }
    }
}