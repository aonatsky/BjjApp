using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TRNMNT.Controllers
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
        public IActionResult Post([FromBody]ILogModel log)
        {
            
            logger.LogError(log.Message);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {

            return Ok();
        }
        public interface ILogModel
        {
            string LogLevel { get; set; }
            string Message { get; set; }
        }
    }
}
