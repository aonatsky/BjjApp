using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.Team;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : BaseController
    {

        #region Dependencies

        private readonly ITeamService _teamService;

        #endregion

        #region .ctor

        public TeamController(
            IEventService eventService,
            ILogger<TeamController> logger,
            IUserService userService,
            ITeamService teamService,
            IAppDbContext context,
            IConfiguration configuration
        ) : base(logger, userService, eventService, context,configuration)
        {
            _teamService = teamService;
        }

        #endregion

        #region Public Methods

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTeams()
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                if (GetEventId() != null)
                {
                    var data = await _teamService.GetTeamsAsync();
                    return Success(data);
                }
                return NotFoundResponse();
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ProcessTeamRegistration([FromBody] TeamModelFull model)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var eventId = GetEventId();
                if (eventId != null)
                {
                    var user = await GetUserAsync();
                    var callbackUrl = Url.Action("ConfirmPayment", "Payment", null, "http");
                    var redirectUrl = $"{Request.Host}/event/team-registration-complete";
                    var result = await _teamService.ProcessTeamRegistrationAsync(GetFederationId().Value, model, callbackUrl, redirectUrl, await GetUserAsync());
                    return Success(result);
                }
                return (null, HttpStatusCode.NotFound);
            });
        }
        #endregion
    }
}

