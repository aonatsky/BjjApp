using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;

namespace TRNMNT.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger):base(logger)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
