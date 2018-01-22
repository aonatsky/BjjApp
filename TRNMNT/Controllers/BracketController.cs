using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> CreateBracket(Guid weightDivisionId)
        {
            return await HandleRequestWithDataAsync(async () =>
                {
                    var bracketModel = await _bracketService.GetBracketAsync(weightDivisionId);
                    if (bracketModel != null)
                    {
                        return Success(bracketModel);
                    }
                    return NotFoundResponse();
                });
        }

        [HttpGet("[action]/{weightDivisionId}")]
        [Authorize]
        public async Task<IActionResult> DownloadFile(Guid weightDivisionId)
        {
            return await HandleRequestWithFileAsync(async () =>
            {
                var file = await _bracketService.GetBracketFileAsync(weightDivisionId);
                return Success<CustomFile>(file);
            });
        }

        #endregion
    }
}