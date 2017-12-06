using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            IAppDbContext context
        ) : base(logger, userService, eventService, context)
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

        #endregion
    }
}

