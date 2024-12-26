using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Facades;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.ResponseMessage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Devshift.Authentication.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersFacade _facade;
        public UsersController(ILogger<UsersController> logger, IUsersFacade facade)
        {
            _logger = logger;
            _facade = facade;
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegisterActivate>> Register([FromHeader(Name = "x-system-key")] string SystemId, [FromBody] Register req)
        {
            _logger.LogInformation("System: {@SystemId}", new { SystemId });
            var resp = await _facade.Register(req);
            if (resp.Message == Message.Created)
            {
                return StatusCode(201, resp);
            }
            return BadRequest();
        }
    }
}