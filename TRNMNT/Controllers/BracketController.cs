using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers 
{
    [Produces("application/json")]
    [Route("api/Bracket")]
    public class BracketController : BaseController
    {
        #region dependencies
        private readonly IBracketService _bracketService; 
        #endregion

        public BracketController(
            IEventService eventService,
            ILogger<PaymentController> logger,
            IUserService userService,
            IAppDbContext context,
            IBracketService bracketService)
            : base(logger, userService, eventService, context)
        {
            _bracketService = bracketService;
        }
        
        #region Public Methods

        [HttpGet("[action]/{weightDivisionId}")]
        [Authorize]
        public async Task<IActionResult> CreateBracket([FromQuery] Guid weightDivisionId)
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                var bracket = await _bracketService.CreateBracket(weightDivisionId);
                return bracket != null ? Success(bracket) : NotFoundResponse();
            });
        }

        #endregion
    }
}