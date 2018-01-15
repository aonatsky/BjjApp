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
using TRNMNT.Core.Services.Interface;
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

        protected IActionResult HandleRequest(Action action)
        {
            return HandleRequest(() =>
            {
                action();
                return HttpStatusCode.OK;
            });
        }

        protected IActionResult HandleRequest(Func<HttpStatusCode> action)
        {
            try
            {
                var code = action();
                return StatusCode((int)code);
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        protected async Task<IActionResult> HandleRequestAsync(Func<Task<HttpStatusCode>> action, bool checkEventId = false, bool checkFederationId = false)
        {
            try
            {
                if (checkEventId && !_eventId.HasValue || checkFederationId && !_federationId.HasValue)
                {
                    return NotFound();
                }
                var code = await action();
                if (code != HttpStatusCode.OK)
                {
                    return StatusCode((int)code);
                }
                await _context.SaveAsync();
                return Ok();
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        protected async Task<IActionResult> HandleRequestAsync(Func<Task> action, bool checkEventId = false, bool checkFederationId = false)
        {
            return await HandleRequestAsync(async () =>
            {
                await action();
                return HttpStatusCode.OK;
            }, checkEventId, checkFederationId);
        }

        protected async Task<IActionResult> HandleRequestAsync(Func<HttpStatusCode> action, bool checkEventId = false, bool checkFederationId = false)
        {
            try
            {
                if ((checkEventId && !_eventId.HasValue) || (checkFederationId && !_federationId.HasValue))
                {
                    return NotFound();
                }
                var code = action();
                if (code != HttpStatusCode.OK)
                {
                    return StatusCode((int)code);
                }
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
            return await HandleRequestAsync(() =>
            {
                action();
                return HttpStatusCode.OK;
            }, checkEventId, checkFederationId);
        }
      
        protected async Task<IActionResult> HandleRequestWithDataAsync<T>(Func<Task<(T Response, HttpStatusCode Code)>> action, bool checkEventId = false, bool checkFederationId = false)
        {
            try
            {
                if (checkEventId && !_eventId.HasValue || checkFederationId && !_federationId.HasValue)
                {
                    return NotFound();
                }
                var result = await action();
                if (result.Code != HttpStatusCode.OK)
                {
                    return StatusCode((int)result.Code);
                }
                await _context.SaveAsync();
                return Ok(JsonConvert.SerializeObject(result.Response, JsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
      
        protected async Task<IActionResult> HandleRequestWithDataAsync<T>(Func<Task<T>> action, bool checkEventId = false, bool checkFederationId = false)
        {
            return await HandleRequestWithDataAsync(async () => (await action(), HttpStatusCode.OK), checkEventId, checkFederationId);
        }

        protected async Task<IActionResult> HandleRequestWithDataAsync<T>(Func<(T Response, HttpStatusCode Code)> action, bool checkEventId = false, bool checkFederationId = false)
        {
            try
            {
                if (checkEventId && !_eventId.HasValue || checkFederationId && !_federationId.HasValue)
                {
                    return NotFound();
                }
                var result = action();
                if (result.Code != HttpStatusCode.OK)
                {
                    return StatusCode((int)result.Code);
                }
                await _context.SaveAsync();
                return Ok(JsonConvert.SerializeObject(result.Response, JsonSerializerSettings));
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        protected async Task<IActionResult> HandleRequestWithDataAsync<T>(Func<T> action, bool checkEventId = false, bool checkFederationId = false)
        {
            return await HandleRequestWithDataAsync(() => (action(), HttpStatusCode.OK), checkEventId, checkFederationId);
        }

        protected (object, HttpStatusCode) NotFoundResponse()
        {
            return (null, HttpStatusCode.NotFound);
        }

        protected (T, HttpStatusCode) Success<T>(T value)
        {
            return (value, HttpStatusCode.OK);
        }

        #endregion
    }
}
