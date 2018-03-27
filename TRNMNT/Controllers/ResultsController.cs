using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{

    [Route("api/Results")]
    public class ResultsController : BaseController
    {
        private readonly IResultsService _resultsService;

        public ResultsController(IResultsService resultsService, ILogger<ResultsController> logger, IUserService userService, IEventService eventService, IAppDbContext context) : base(logger, userService, eventService, context)
        {
            _resultsService = resultsService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetTeamResults([FromBody] List<string> categoryIds)
        {
            return await HandleRequestWithDataAsync(async () => await _resultsService.GetTeamResultsByCategoriesAsync(categoryIds.Select(c => new Guid(c))));
        }
    }
}