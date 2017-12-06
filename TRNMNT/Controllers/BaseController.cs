using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TRNMNT.Core.Services;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;

namespace TRNMNT.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Dependencies

        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly IAppDbContext _context;
        protected JsonSerializerSettings JsonSerializerSettings;

        #endregion

        #region Properties

        private Guid? _eventId;
        private Guid? _federationId;
        private User _user;

        #endregion

        public BaseController(ILogger logger, IUserService userService, IEventService eventService, IAppDbContext context)
        {
            _logger = logger;
            _userService = userService;
            _eventService = eventService;
            _context = context;
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore // ignore null values
            };
        }
        protected void HandleException(Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        protected async Task<User> GetUserAsync()
        {
            if (_user == null)
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    _user = await _userService.GetUserAsync(userIdClaim.Value);
                }
            }
            return _user;
        }

        private async Task ParseEventSubdomain()
        {
            var fullAddress = HttpContext.Request.Headers["Host"].FirstOrDefault().Split('.');
            if (fullAddress.Length == 2)
            {
                var eventSubdomain = fullAddress[0];
                var host = fullAddress[1];
                var eventId = await _eventService.GetEventIdAsync(eventSubdomain);
                if (eventId != null)
                {
                    _eventId = eventId;
                }
                else
                {
                    HttpContext.Response.Redirect($"{HttpContext.Request.Scheme}://{host}/");
                }
            }

            //hadrcoded federationid
            _federationId = new Guid("673ea3ce-2530-48c0-b84c-a3de492cab25");

        }

        protected Guid? GetEventId()
        {
            return _eventId;
        }

        protected Guid? GetFederationId()
        {
            return _federationId;
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
                await _context.SaveAsync();
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
                await _context.SaveAsync();
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
                if (checkEventId && !_eventId.HasValue || checkFederationId && !_federationId.HasValue)
                {
                    return NotFound();
                }
                var result = await action();
                await _context.SaveAsync();
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
                if (checkEventId && !_eventId.HasValue || checkFederationId && !_federationId.HasValue)
                {
                    return NotFound();
                }
                var result = action();
                await _context.SaveAsync();
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
