using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
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
        private readonly ICategoryService _categoryService;
        private readonly IMatchService _matchService;

        #endregion

        public BracketController(
            IEventService eventService,
            ILogger<BracketController> logger,
            IUserService userService,
            IAppDbContext context,
            IWeightDivisionService weightDivisionService,
            IBracketService bracketService,
            ICategoryService categoryService,
            IMatchService matchService,
            IConfiguration configuration) : base(logger, userService, eventService, context, configuration)
        {
            _bracketService = bracketService;
            _categoryService = categoryService;
            this._matchService = matchService;
        }

        #region Public Methods

        [HttpGet("[action]/{weightDivisionId}")]
        public async Task<IActionResult> CreateBracket(Guid weightDivisionId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var bracketModel = await _bracketService.GetBracketModelAsync(weightDivisionId);
                if (bracketModel != null)
                {
                    return Success(bracketModel);
                }

                return NotFoundResponse();
            });
        }

        [HttpGet("[action]/{eventId}")]
        [Authorize(Roles = "FederationOwner, Owner, Admin")]
        public async Task<IActionResult> AreBracketsCreated(Guid eventId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                return await _matchService.AreMatchesCreatedForEventAsync(eventId);
            });
        }

        [HttpPost("[action]/{eventId}")]
        [Authorize(Roles = "FederationOwner, Owner, Admin")]
        public async Task<IActionResult> CreateBrackets(Guid eventId)
        {
            return await HandleRequestAsync(async() =>
            {
                await _bracketService.CreateBracketsForEventAsync(eventId);
            });
        }

        [HttpPost("[action]/{eventId}")]
        [Authorize(Roles = "FederationOwner, Owner, Admin")]
        public async Task<IActionResult> DeleteBrackets(Guid eventId)
        {
            return await HandleRequestAsync(async() =>
            {
                await _bracketService.DeleteBracketsForEventAsync(eventId);
            });
        }

        [HttpGet("[action]/{weightDivisionId}")]
        [Authorize(Roles = "FederationOwner, Owner, Admin")]
        public async Task<IActionResult> RunBracket(Guid weightDivisionId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var bracketModel = await _bracketService.RunWeightDivisionAsync(weightDivisionId);
                if (bracketModel != null)
                {
                    return Success(bracketModel);
                }

                return NotFoundResponse();
            });
        }

        [HttpGet("[action]/{weightDivisionId}")]
        [Authorize(Roles = "FederationOwner, Owner, Admin")]
        public async Task<IActionResult> DownloadFile(Guid weightDivisionId)
        {
            return await HandleRequestWithFileAsync(async() =>
            {
                var file = await _bracketService.GetBracketFileAsync(weightDivisionId);
                return Success<CustomFile>(file);
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]")]
        public async Task<IActionResult> UpdateBracket([FromBody] BracketModel bracketModel)
        {
            return await HandleRequestAsync(async() =>
            {
                await _bracketService.UpdateBracket(bracketModel);
                return HttpStatusCode.OK;
            });
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetBracketsByCategory(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async() =>
                Success(await _bracketService.GetBracketsByCategoryAsync(categoryId)));
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetParticipantsForAbsoluteDivision(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var winners = await _bracketService.GetParticipantsForAbsoluteDivisionAsync(categoryId);
                return Success(winners);
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> IsCategoryCompleted(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var isCompleted = await _categoryService.IsCategoryCompletedAsync(categoryId);
                return Success(isCompleted);
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]")]
        public async Task<IActionResult> ManageAbsoluteWeightDivision([FromBody] CreateAbsoluteDivisionModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _bracketService.EditAbsoluteWeightDivisionAsync(model);
                return HttpStatusCode.OK;
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]")]
        public async Task<IActionResult> SetRoundResult([FromBody] MatchResultModel roundResultModel)
        {
            return await HandleRequestAsync(async() =>
            {
                await _bracketService.SetRoundResultAsync(roundResultModel);
                return HttpStatusCode.OK;
            });

        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]")]
        public async Task<IActionResult> SetBracketResult([FromBody] BracketResultModel bracketResultModel)
        {
            return await HandleRequestAsync(async() =>
            {
                await _bracketService.SetBracketResultAsync(bracketResultModel);
                return HttpStatusCode.OK;
            });

        }

        #endregion
    }
}