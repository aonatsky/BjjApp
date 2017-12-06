using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services;
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
            IAppDbContext context)
            : base(logger, userService, eventService, context)
        {
            _paymentService = paymentService;
            _eventService = eventService;
        }

        #endregion

        #region Public Methods

        [HttpPost("[action]/{eventId}")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentDataModel model)
        {
            return await HandleRequestAsync(async () =>
            {
                await _paymentService.ConfirmPaymentAsync(model);
            });
        }

        #endregion
    }
}

