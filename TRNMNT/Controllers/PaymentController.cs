using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController
    {
        #region Dependencies

        private readonly IPaymentService _paymentService;
        private readonly IEventService _eventService;

        #endregion

        #region .ctor

        public PaymentController(
            IEventService eventService,
            ILogger<PaymentController> logger,
            IUserService userService,
            IPaymentService paymentService,
            IConfiguration configuration,
            IAppDbContext context) : base(logger, userService, eventService, context, configuration)
        {
            _paymentService = paymentService;
            _eventService = eventService;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentDataModel model)
        {
            Logger.LogWarning($"model data {model.Data}");
            return Ok();
            //return await HandleRequestAsync(async() => await _paymentService.ConfirmPaymentAsync(model));
        }

        [AllowAnonymous, HttpGet("[action]/{orderId}")]
        public async Task<IActionResult> CheckPaymentStatus(string orderId)
        {
            return await HandleRequestAsync(async () => await _paymentService.GetPaymentStatusAsync(new Guid(orderId)));
        }
        #endregion
    }
}