﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Bracket;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Web.Hubs;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class BracketController : BaseController
    {
        #region dependencies

        private readonly IHubContext<RunEventHub> _hubContext;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly IBracketService _bracketService;

        #endregion

        public BracketController(
            IEventService eventService,
            ILogger<BracketController> logger,
            IUserService userService,
            IAppDbContext context,
            IHubContext<RunEventHub> hubContext,
            IWeightDivisionService weightDivisionService,
            IBracketService bracketService)
            : base(logger, userService, eventService, context)
        {
            _hubContext = hubContext;
            _weightDivisionService = weightDivisionService;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateBracket([FromBody] BracketModel bracketModel)
        {
            return await HandleRequestAsync(async () =>
            {
                await _bracketService.UpdateBracket(bracketModel);
                return HttpStatusCode.OK;
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> FinishRound([FromBody] Guid weightDivisionId)
        {
            return await HandleRequestAsync(async () =>
            {
                var clients = _hubContext.Clients;
                if (clients != null)
                {
                    var bracketModel = await _bracketService.GetBracketAsync(weightDivisionId);
                    if (bracketModel != null)
                    {
                        var divisionId = weightDivisionId.ToString().ToUpperInvariant();
                        var refreshModel = new RefreshBracketModel
                        {
                            WeightDivisionId = divisionId,
                            Bracket = bracketModel
                        };
                        await clients.Group(divisionId).InvokeAsync("BracketRoundsUpdated", refreshModel);
                    }
                }
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
        public async Task<IActionResult> GetWinners(Guid categoryId)
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                var weightDivisions = await _weightDivisionService.GetWeightDivisionsByCategoryIdAsync(categoryId);
                var winners = await _bracketService.GetWinnersAsync(weightDivisions.Select(w => new Guid(w.WeightDivisionId)));
                return Success(winners);
            });
        }

        #endregion
    }
}