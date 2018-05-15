using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class WeightDivisionController : BaseController
    {
        private readonly IWeightDivisionService _weightDivisionService;

        public WeightDivisionController(
            ILogger<WeightDivisionController> logger,
            IWeightDivisionService weightDivisionService,
            IUserService userService,
            IEventService eventService,
            IAppDbContext context)
            : base(logger, userService, eventService, context)
        {
            _weightDivisionService = weightDivisionService;
        }


        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetWeightDivisionsByCategory(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async () => await _weightDivisionService.GetWeightDivisionModelsByCategoryIdAsync(categoryId));
        }

        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetWeightDivisionsByEvent(Guid eventId)
        {
            return await HandleRequestWithDataAsync(async () => await _weightDivisionService.GetWeightDivisionModelsByEventIdAsync(eventId, true));
        }
    }

}
