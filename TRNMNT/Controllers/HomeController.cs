using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;

namespace TRNMNT.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IFighterService fighterService,ILogger<HomeController> logger):base(logger)
        {
            var test1 = fighterService;
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
