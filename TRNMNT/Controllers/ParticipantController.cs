using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Services;
using TRNMNT.Data.Context;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class ParticipantController : BaseController
    {
        #region Dependencies

        private readonly IParticipantService _participantService;
        private readonly IEventService _eventService;
        private readonly IParticipantRegistrationService _participantRegistrationService;

        #endregion

        #region .ctor
        public ParticipantController(IEventService eventService,
            ILogger<TeamController> logger,
            IUserService userService,
            IParticipantService participantService,
            IPaymentService paymentService,
            IOrderService orderService,
            IParticipantRegistrationService participantRegistrationService,
            IAppDbContext context)
            : base(logger, userService, eventService, context)
        {

            _participantService = participantService;
            _participantRegistrationService = participantRegistrationService;
            _eventService = eventService;
        }

        #endregion
        
        #region Public Methods

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> IsParticipantExist([FromBody]ParticipantRegistrationModel model)
        {
            try
            {
                if (GetEventId() != null)
                {
                    var result = await _participantService.IsParticipantExistsAsync(model, GetEventId().Value);
                    return Ok(JsonConvert.SerializeObject(result, JsonSerializerSettings));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ProcessParticipantRegistration([FromBody]ParticipantRegistrationModel model)
        {
            try
            {
                var eventId = GetEventId();
                if (eventId != null)
                {
                    var user = await GetUserAsync();
                    var callbackUrl = $"{Request.Host}{Url.Action("ConfirmPayment", "Payment")}";
                    var result = await _participantRegistrationService.ProcessParticipantRegistrationAsync(eventId.Value, model, callbackUrl);
                    return Ok(JsonConvert.SerializeObject(result, JsonSerializerSettings));
                }
                return NotFound();
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

        #endregion
    }
}

