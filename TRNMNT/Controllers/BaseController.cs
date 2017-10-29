using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using TRNMNT.Core.Services;
using TRNMNT.Data.Entities;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TRNMNT.Web.Const;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TRNMNT.Web.Controllers
{
    public class BaseController : Controller
    {
        private ILogger logger;
        private readonly IHttpContextAccessor contextAccessor;
        private IUserService userService;
        protected JsonSerializerSettings jsonSerializerSettings;


        private User user;

        public BaseController(ILogger logger, IHttpContextAccessor context, IUserService userService)
        {
            this.contextAccessor = context;
            this.logger = logger;
            this.userService = userService;

            jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore // ignore null values
            };
        }
        protected void HandleException(Exception ex)
        {
            logger.LogError(ex.Message);
        }

        protected async Task<User> GetUserAsync()
        {
            if (user == null)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null)
                {
                    user = await userService.GetUserAsync(userIdClaim.Value);
                }
                
            }
            return user;
        }

        protected string GetEventId()
        {
            return this.RouteData.Values[AppConstants.RouteDataKeyEventId] as string;
        }

        protected string GetFederationId()
        {
            return this.RouteData.Values[AppConstants.RouteDataKeyFederationId] as string;
        }

        private string GetHomePage()
        {
            return RouteData.Values[AppConstants.RouteDataKeyHomePage] as string;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ViewBag.HomePage = GetHomePage();
            ViewBag.EventId = GetEventId();
            ViewBag.FederationId = GetFederationId();
            return base.OnActionExecutionAsync(context, next);
        }

    }
}
