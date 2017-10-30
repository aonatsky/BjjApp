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
        private readonly HttpContext httpContext;
        private IUserService userService;
        private IEventService eventService;
        protected JsonSerializerSettings jsonSerializerSettings;

        private Guid? eventId;

        private User user;

        public BaseController(ILogger logger, IUserService userService, IEventService eventService)
        {
            this.logger = logger;
            this.userService = userService;
            this.eventService = eventService;

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

        private async Task ParseEventSubdomain()
        {
            var fullAddress = HttpContext.Request.Headers["Host"].FirstOrDefault().Split('.');
            if (fullAddress.Length == 2)
            {
                var eventSubdomain = fullAddress[0];
                var host = fullAddress[1];
                var eventId = await eventService.GetEventIdAsync(eventSubdomain);
                if (eventId != null)
                {
                    this.eventId = eventId;
                }
                else
                {
                    HttpContext.Response.Redirect($"{HttpContext.Request.Scheme}://{host}/");
                }
            }
        }

        protected Guid? GetEventId()
        {
            return eventId;
        }

        protected string GetFederationId()
        {
            return this.RouteData.Values[AppConstants.RouteDataKeyFederationId] as string;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await ParseEventSubdomain();
            await base.OnActionExecutionAsync(context, next);
        }

    }
}
