﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> CreateBracket([FromQuery] Guid weightDivisionId)
        {
            return await HandleRequestWithDataAsync(async () =>
                {
                    var bracketModel = await _bracketService.CreateBracketAsync(weightDivisionId);
                    if (bracketModel != null)
                    {
                        return Success(bracketModel);
                    }
                    return NotFoundResponse();
                }, false, false);
        }

        #endregion
    }
}