using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

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
            return await HandleRequestWithDataAsync(async () => await _weightDivisionService.GetWeightDivisionsByEventIdAsync(eventId));
        }

        //public override IQueryable<WeightDivision> ModifyQuery(string key, string value, IQueryable<WeightDivision> query)
        //{
        //    switch (key)
        //    {
        //        case "categoryId":
        //            {
        //                query = query.Where(w => w.CategoryId == Guid.Parse(value));
        //                break;
        //            }
        //    }

        //    return query;
        //}
    }

}
