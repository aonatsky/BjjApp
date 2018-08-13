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
        private readonly IFederationService _federationService;
        private readonly IUserService _userService;

        #endregion

        #region .ctor

        public TeamController(
            IEventService eventService,
            ILogger<TeamController> logger,
            IUserService userService,
            ITeamService teamService,
            IAppDbContext context,
            IFederationService federationService,
            IConfiguration configuration
        ) : base(logger, userService, eventService, context, configuration)
        {
            _userService = userService;
            _teamService = teamService;
            _federationService = federationService;
        }

        #endregion

        #region Public Methods

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTeamsForEvent()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var data = await _teamService.GetTeamsForEventAsync(GetFederationId().Value);
                return Success(data);
            }, true, true);
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpGet("[action]")]
        public async Task<IActionResult> GetTeamsForAdmin()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var data = await _teamService.GetTeamsForAdminAsync(GetFederationId().Value);
                return Success(data);
            }, false, true);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTeamRegistrationPrice()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                if (GetEventId() != null)
                {
                    var data = await _federationService.GetTeamRegistrationPriceAsync(GetFederationId().Value);
                    return Success(data);
                }
                return NotFoundResponse();
            });
        }

        [Authorize(Roles = "Admin, TeamOwner"), HttpGet("[action]")]
        public async Task<IActionResult> GetTeamMembers()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var data = await _teamService.GetAthletes((await GetUserAsync()).Id);
                return Success(data);
            }, false, true);
        }

        [Authorize(Roles = "Admin, TeamOwner"),HttpPost("[action]/{userId}")]
        public async Task<IActionResult> ApproveTeamMembership(string userId)
        {
            return await HandleRequestAsync(async() =>
            {
                await _userService.ApproveTeamMembershipAsync(userId);
            }, false, true);
        }

        [Authorize(Roles = "Admin, TeamOwner"),HttpPost("[action]/{userId}")]
        public async Task<IActionResult> DeclineTeamMembership(string userId)
        {
            return await HandleRequestAsync(async() =>
            {
                await _userService.DeclineTeamMembershipAsync(userId);
            }, false, true);
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