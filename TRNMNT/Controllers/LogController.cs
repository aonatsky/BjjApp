using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {

        private readonly ILogger logger;

        public LogController(ILogger<LogController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody]LogModel log)
        {

            if (log != null)
            {
                logger.LogError(log.Message);
            }
            else
            {
                logger.LogError("An Error Occured");
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
            public string Level { get; set; }
            public string Message { get; set; }
        }
    }
}
