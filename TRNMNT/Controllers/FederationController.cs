using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "FederationOwner,Admin"), HttpGet("[action]")]
        public async Task<IActionResult> GetFederation()
        {
            return await HandleRequestWithDataAsync(async() =>
            {

                var user = await GetUserAsync();
                var federationModel = await _federationService.GetFederationModelAsync(GetFederationId().Value, user.Id, await UserService.IsAdminAsync(user));
                if (federationModel == null)
                {
                    return NotFoundResponse();
                }
                return Success(federationModel);
            }, false, true);
        }

        [Authorize(Roles = "FederationOwner,Admin"), HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> UpdateFederation([FromBody] FederationModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                var user = await GetUserAsync();
                await _federationService.UpdateFederationAsync(model, GetFederationId().Value, user.Id, await UserService.IsAdminAsync(user));
            });
        }

        #endregion
    }
}