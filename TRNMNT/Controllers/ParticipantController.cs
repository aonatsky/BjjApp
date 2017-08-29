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
    public class ParticipantController : BaseController
    {
        IHttpContextAccessor httpContextAccessor;
        private IParticipantService participantService;
        private IPaymentService paymentService;
        private IEventService eventService;

        public ParticipantController(IEventService eventService, ILogger<TeamController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService, IParticipantService participantService, IPaymentService paymentService)
            : base(logger, httpContextAccessor, userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.participantService = participantService;
            this.paymentService = paymentService;
            this.eventService = eventService;
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

