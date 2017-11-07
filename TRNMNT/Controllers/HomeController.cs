using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using TRNMNT.Web.Core.Services;
using TRNMNT.Web.Const;
using TRNMNT.Data.Context;

namespace TRNMNT.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger, 
            IUserService userService,
            IEventService eventService,
            IAppDbContext context
            ) : base(logger, userService, eventService, context)
        {
        }
        public IActionResult Index()
        {
            try
            {
                ViewBag.HomePage = GetHomePage();
                return View();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        private string GetHomePage()
        {
            if (GetEventId() != null)
            {
                return AppConstants.PageUrlEventInfo;
            }
            else
            {
                return AppConstants.PageUrlDefault;
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
