using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Web.Core.Services;

namespace TRNMNT.Web.Controllers
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
