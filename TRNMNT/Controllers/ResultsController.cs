using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{

    [Route("api/Results")]
    public class ResultsController : BaseController
    {
        private readonly IBracketService _bracketService;
        

        public ResultsController(IBracketService bracketService, ILogger<ResultsController> logger, IUserService userService, IEventService eventService, IAppDbContext context, IConfiguration configuration)
         : base(logger, userService, eventService, context, configuration)
        {
            _bracketService = bracketService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetTeamResults([FromBody] List<string> categoryIds)
        {
            return await HandleRequestWithDataAsync(async () => await _bracketService.GetTeamResultsByCategoriesAsync(categoryIds.Select(c => new Guid(c))));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetPersonalResultsFile([FromBody] List<string> categoryIds)
        {
            return await HandleRequestWithFileAsync(async () =>
            {
                var file = await _bracketService.GetPersonalResultsFileByCategoriesAsync(categoryIds.Select(c => new Guid(c)));
                return Success<CustomFile>(file);
            });
        }
    }
}