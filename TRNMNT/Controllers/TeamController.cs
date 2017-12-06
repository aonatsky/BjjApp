using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Services;
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
            try
            {
                if (GetEventId() != null)
                {
                    var data = await _teamService.GetTeams();
                    return Ok(JsonConvert.SerializeObject(data, JsonSerializerSettings));
                }
                return NotFound();
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }
}

