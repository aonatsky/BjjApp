using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Web.Helpers;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        #region Dependencies

        private readonly ILogger _logger;

        #endregion

        #region .ctor

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        #endregion

        [HttpPost]
        public IActionResult Post([FromBody]LogModel log)
        {

            if (log != null)
            {
                _logger.Log(log.Level, log.Message);
            }
            else
            {
                _logger.LogError("An Error Occured");
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        public class LogModel
        {
            public LogLevel Level { get; set; }
            public string Message { get; set; }
        }
    }
}
