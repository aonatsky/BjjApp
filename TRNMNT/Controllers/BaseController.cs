using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace TRNMNT.Web.Controllers
{
    public class BaseController : Controller
    {
        private ILogger logger;
        private readonly IHttpContextAccessor context;

        public BaseController(ILogger logger, IHttpContextAccessor context)
        {
            this.context = context;
            this.logger = logger;
        }
        protected void HandleException(Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}
