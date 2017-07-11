using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using TRNMNT.Web.Core.Services;

namespace TRNMNT.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(logger, httpContextAccessor, userService)
        {
        }
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        public IActionResult Error()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }
    }
}
