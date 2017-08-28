using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Entities;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TRNMNT.Data.Repositories;
using System.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : BaseController
    {
        IHttpContextAccessor httpContextAccessor;
        ITeamService teamService;

        public TeamController(IEventService eventService, ILogger<TeamController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService, ITeamService teamService)
        : base(logger, httpContextAccessor, userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.teamService = teamService;
        }

        [Authorize]
        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetTeams(string eventId)
        {
            try
            {
                var data = await teamService.GetTeams();
                return Ok(JsonConvert.SerializeObject(data, jsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }

    }
}

