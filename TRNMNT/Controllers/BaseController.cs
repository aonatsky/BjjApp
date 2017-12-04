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
using Microsoft.AspNetCore.Mvc.Filters;
using TRNMNT.Data.Context;
using System.Net;
using System.Security.Claims;

namespace TRNMNT.Web.Controllers
{
    public class BaseController : Controller
    {
        private ILogger logger;
        private IUserService userService;
        private IEventService eventService;
        private readonly IAppDbContext context;
        protected JsonSerializerSettings jsonSerializerSettings;

        private Guid? eventId;
        private Guid? federationId;
        private User user;

        public BaseController(ILogger logger, IUserService userService, IEventService eventService, IAppDbContext context)
        {
            this.logger = logger;
            this.userService = userService;
            this.eventService = eventService;
            this.context = context;
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
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
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

            //hadrcoded federationid
            federationId = new Guid("673ea3ce-2530-48c0-b84c-a3de492cab25");

        }

        protected Guid? GetEventId()
        {
            return eventId;
        }

        protected Guid? GetFederationId()
        {
            return federationId;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await ParseEventSubdomain();
            await base.OnActionExecutionAsync(context, next);
        }


        #region Protected Methods

        protected async Task<IActionResult> HandleRequestAsync(Func<Task> action)
        {
            try
            {
                await action();
                await context.SaveAsync();
                return Ok();
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        protected async Task<IActionResult> HandleRequestAsync(Action action, bool checkEventId = false, bool checkFederationId = false)
        {

            try
            {
                action();
                await context.SaveAsync();
                return Ok();

            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

      
        protected async Task<IActionResult> HandleRequestWithDataAsync<T>(Func<Task<T>> action, bool checkEventId = false, bool checkFederationId = false)
        {
            try
            {
                if ((checkEventId && !eventId.HasValue) || (checkFederationId && !federationId.HasValue))
                {
                    return NotFound();
                }
                var result = await action();
                await context.SaveAsync();
                return Ok(result);

            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        protected async Task<IActionResult> HandleRequestWithDataAsync<T>(Func<T> action, bool checkEventId = false, bool checkFederationId = false)
        {
            try
            {
                if ((checkEventId && !eventId.HasValue) || (checkFederationId && !federationId.HasValue))
                {
                    return NotFound();
                }
                var result = action();
                await context.SaveAsync();
                return Ok(result);

            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }



        #endregion

    }
}
