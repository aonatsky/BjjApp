using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class ParticipantController : BaseController
    {
        IHttpContextAccessor httpContextAccessor;
        private IParticipantService participantService;
        private IEventService eventService;
        private IParticipantRegistrationService participantRegistrationService;

        public ParticipantController(IEventService eventService, ILogger<TeamController> logger, 
            IHttpContextAccessor httpContextAccessor, IUserService userService, IParticipantService participantService, 
            IPaymentService paymentService, IOrderService orderService, IParticipantRegistrationService participantRegistrationService)
            : base(logger, httpContextAccessor, userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.participantService = participantService;
            this.participantRegistrationService = participantRegistrationService;
            this.eventService = eventService;
        }
  


        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> IsParticipantExist([FromBody]ParticipantRegistrationModel model)
        {
            try
            {
                var result = await participantService.IsParticipantExistsAsync(model);
                return Ok(JsonConvert.SerializeObject(result, jsonSerializerSettings));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult>ProcessParticipantRegistration([FromBody]ParticipantRegistrationModel model)
        {
            try
            {

                var user = await GetUserAsync();
                var callbackUrl = $"{Request.Host.ToString()}{Url.Action("ConfirmPayment")}/{model.EventId}";
                var result = await participantRegistrationService.ProcessParticipantRegistrationAsync(model,user.Id,callbackUrl);
                return Ok(JsonConvert.SerializeObject(result, jsonSerializerSettings));
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost("[action]/{eventId}")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentDataModel model)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}

