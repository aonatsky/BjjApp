using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TRNMNT.Web.Controllers
{
    public class BaseController : Controller
    {
        private ILogger logger;
        public BaseController(ILogger logger)
        {
            this.logger = logger;
        }
        protected void HandleException(Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }
}
