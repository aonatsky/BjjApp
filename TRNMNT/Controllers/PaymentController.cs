using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TRNMNT.Core.Model;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController
    {
        IHttpContextAccessor httpContextAccessor;
        IPaymentService paymentService;
        IEventService eventService;

        public PaymentController(IEventService eventService, ILogger<PaymentController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService, IPaymentService paymentService)
        : base(logger, httpContextAccessor, userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.paymentService = paymentService;
            this.eventService = eventService;
        }

        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetPaymentDataForParticipant(string eventId)
        {
            try
            {
                var user = await GetUserAsync();
                var callbackUrl = $"{Request.Host.ToString()}{Url.Action("ConfirmPayment")}/{eventId}";
                var data = paymentService.GetPaymentDataModel(await eventService.GetPrice(Guid.Parse(eventId), user.Id), callbackUrl);
                return Ok(JsonConvert.SerializeObject(data, jsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

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

