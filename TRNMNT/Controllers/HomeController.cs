using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Web.Const;

namespace TRNMNT.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Public Methods

        public HomeController(ILogger<HomeController> logger,
            IUserService userService,
            IEventService eventService,
            IAppDbContext context,
            IConfiguration configuration
            ) : base(logger, userService, eventService, context, configuration) { }

        #endregion

        #region Public Methods

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

        #endregion
    }
}
