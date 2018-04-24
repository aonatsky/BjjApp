using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    public class RoundController : BaseController
    {
        private readonly IRoundService _roundService;

        #region .ctor

        public RoundController(ILogger logger, IUserService userService, IEventService eventService, IAppDbContext context, IRoundService roundService) : base(logger, userService, eventService, context)
        {
            _roundService = roundService;
        }

        #endregion

        #region Public Methods

        [HttpPost("[action]")]
        public async Task CompleteRound([FromBody]RoundResultModel roundResultModel)
        {
            try
            {
                await _roundService.SetRoundResultAsync(roundResultModel);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        #endregion
    }
}
