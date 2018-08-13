using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Model.Federation;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class FederationController : BaseController
    {
        #region Dependencies

        private readonly IFederationService _federationService;

        #endregion

        #region .ctor

        public FederationController(
            ILogger<CategoryController> logger,
            IFederationService federationService,
            IHttpContextAccessor httpContextAccessor,
            IEventService eventService,
            IUserService userService,
            IConfiguration configuration,
            IAppDbContext context) : base(logger, userService, eventService, context, configuration)
        {
            _federationService = federationService;
        }

        #endregion

        #region Public Methods

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFederation()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var federationModel = await _federationService.GetFederationModelAsync(GetFederationId().Value, (await GetUserAsync()).Id);
                if (federationModel == null)
                {
                    return NotFoundResponse();
                }
                return Success(federationModel);
            }, false, true);
        }

        [HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> UpdateFederation([FromBody] FederationModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _federationService.UpdateFederationAsync(model, GetFederationId().Value, (await GetUserAsync()).Id);
            });
        }

        #endregion
    }
}