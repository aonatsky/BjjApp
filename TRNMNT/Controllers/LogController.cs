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
        public IActionResult Post([FromBody]string message)
        {
            logger.LogError(message);
            return Ok();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
