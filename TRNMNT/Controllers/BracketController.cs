using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Web.Hubs;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class 
        BracketController : BaseController
    {
        #region dependencies

        private readonly IBracketService _bracketService;

        #endregion

        public BracketController(
            IEventService eventService,
            ILogger<BracketController> logger,
            IUserService userService,
            IAppDbContext context,
            IWeightDivisionService weightDivisionService,
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
                var bracketModel = await _bracketService.GetBracketModelAsync(weightDivisionId);
                if (bracketModel != null)
                {
                    return Success(bracketModel);
                }

                return NotFoundResponse();
            });
        }

        [HttpGet("[action]/{weightDivisionId}")]
        [Authorize]
        public async Task<IActionResult> RunBracket(Guid weightDivisionId)
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                var bracketModel = await _bracketService.RunWeightDivision(weightDivisionId);
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

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateBracket([FromBody] BracketModel bracketModel)
        {
            return await HandleRequestAsync(async () =>
            {
                await _bracketService.UpdateBracket(bracketModel);
                return HttpStatusCode.OK;
            });
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetBracketsByCategory(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async () =>
                Success(await _bracketService.GetBracketsByCategoryAsync(categoryId)));
        }

        [Authorize, HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetParticipnatsForAbsoluteDivision(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                var winners = await _bracketService.GetParticipantsForAbsoluteDivisionAsync(categoryId);
                return Success(winners);
            });
        }

        [Authorize, HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> IsCategoryCompleted(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                var isSelected = await _bracketService.IsCategoryCompletedAsync(categoryId);
                return Success(isSelected);
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ManageAbsoluteWeightDivision([FromBody] CreateAbsoluteDivisionModel model)
        {
            return await HandleRequestAsync(async () =>
            {
                await _bracketService.ManageAbsoluteWeightDivisionAsync(model);
                return HttpStatusCode.OK;
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> SetRoundResult([FromBody]RoundResultModel roundResultModel)
        {
            return await HandleRequestAsync(async () =>
            {
                await _bracketService.SetRoundResultAsync(roundResultModel);
                return HttpStatusCode.OK;
            });
            
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> SetBracketResult([FromBody]BracketResultModel bracketResultModel)
        {
            return await HandleRequestAsync(async () =>
            {
                await _bracketService.SetBracketResultAsync(bracketResultModel);
                return HttpStatusCode.OK;
            });
            
        }

        #endregion
    }
}