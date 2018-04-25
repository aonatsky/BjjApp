using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.Round;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    public class RoundController : BaseController
    {
        #region .ctor

        public RoundController(ILogger logger, IUserService userService, IEventService eventService, IAppDbContext context) : base(logger, userService, eventService, context)
        {
        }

        #endregion

        #region Public Methods

        [HttpPost("[action]")]
        public async Task CompleteRound([FromBody]RoundResultModel roundResultModel)
        {
            try
            {
                
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
