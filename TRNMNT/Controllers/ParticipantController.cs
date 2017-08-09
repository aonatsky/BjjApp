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
using TRNMNT.Core.Model;
using TRNMNT.Web.Core.Enum;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class ParticipantController : CRUDController<Participant>
    {
        IHttpContextAccessor httpContextAccessor;
        private IParticipantService participantService;

        public ParticipantController(IEventService eventService, ILogger<TeamController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService, IRepository<Participant> repository, IParticipantService participantService)
            : base(logger, repository, httpContextAccessor, userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.participantService = participantService;
        }

        public override IQueryable<Participant> ModifyQuery(string key, string value, IQueryable<Participant> query)
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterParticipant([FromBody]ParticipantRegistrationModel model)
        {
            try
            {
                var result = await participantService.RegisterParticipantAsync(model);
                return Ok(JsonConvert.SerializeObject(result, jsonSerializerSettings));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);

            }
        }
    }
}

