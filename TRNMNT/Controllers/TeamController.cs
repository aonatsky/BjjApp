using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : BaseController
    {
        ITeamService teamService;

        public TeamController(IEventService eventService, ILogger<TeamController> logger, IUserService userService, ITeamService teamService)
        : base(logger, userService, eventService)
        {
            this.teamService = teamService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTeams()
        {
            try
            {
                if (GetEventId() != null)
                {
                    var data = await teamService.GetTeams();
                    return Ok(JsonConvert.SerializeObject(data, jsonSerializerSettings));
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }

    }
}

