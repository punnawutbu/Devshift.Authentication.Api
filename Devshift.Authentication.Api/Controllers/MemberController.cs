using System.Threading;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Facades;
using Devshift.Authentication.Api.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Devshift.Authentication.Api.Shared.Utils;

namespace Devshift.Authentication.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMemberFacade _facade;

        public MemberController(ILogger<MemberController> logger, IMemberFacade facade)
        {
            _logger = logger;
            _facade = facade;
        }
        [HttpPost("register")]
        public async Task<ActionResult<RegisterActivate>> Register(
            [FromHeader(Name = "x-system-key")] string SystemId,
            [FromBody] RegisterRequest req,
            CancellationToken ct)
        {
            _logger.LogInformation("System: {@SystemId}", new { SystemId });
            var resp = await _facade.Register(req, SystemId, ct);
            if (resp.Message == Constants.Message.RegisterSuccess)
            {
                return StatusCode(201, resp);
            }
            return BadRequest(resp);
        }
    }
}