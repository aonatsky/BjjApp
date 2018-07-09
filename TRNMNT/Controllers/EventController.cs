using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Authorize(Roles = "FederationOwner, Owner")]
    [Route("api/[controller]")]
    public class EventController : BaseController
    {
        #region Dependencies

        private readonly IEventService _eventService;

        #endregion

        #region .ctor

        public EventController(IEventService eventService, ILogger<EventController> logger, IUserService userService, IAppDbContext context) : base(logger, userService, eventService, context)
        {
            _eventService = eventService;
        }

        #endregion

        #region Public Methods

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventModelFull eventModel)
        {
            return await HandleRequestAsync(async() =>
            {
                //if (await CheckEventOwnerAsync(eventModel.EventId))
                //{
                await _eventService.UpdateEventAsync(eventModel);
                return HttpStatusCode.OK;
                //}
                //return HttpStatusCode.Unauthorized;
            });
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var eventModel = await _eventService.GetFullEventAsync(id);
                if (eventModel != null)
                {
                    return Success(eventModel);
                }
                return NotFoundResponse();
            });
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEventBaseInfo(Guid id)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var eventModel = await _eventService.GetFullEventAsync(id);
                if (eventModel != null)
                {
                    return Success(eventModel);
                }
                return NotFoundResponse();
            });
        }

        [Authorize, HttpGet("[action]")]
        public async Task<EventModelBase[]> GetEventsForOwner()
        {
            Response.StatusCode = (int) HttpStatusCode.OK;
            try
            {

                var events = await _eventService.GetEventsForOwnerAsync((await GetUserAsync()).Id);
                return events.ToArray();

            }
            catch (Exception e)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                HandleException(e);
                return null;
            }
        }

        [AllowAnonymous, HttpGet("[action]")]
        public async Task<IActionResult> GetEventInfo()
        {
            return await HandleRequestWithDataAsync(async() =>(await _eventService.GetEventInfoAsync(GetEventId()), HttpStatusCode.OK), true);
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> IsPrefixExists(string prefix)
        {
            return await HandleRequestAsync(async() =>
            {
                if (await _eventService.IsEventUrlPrefixExistAsync(prefix))
                {
                    return HttpStatusCode.Found;
                }
                return HttpStatusCode.OK;
            });
        }

        [Authorize, HttpPost("[action]/{id}")]
        public async Task<IActionResult> UploadEventImage(IFormFile file, string id)
        {
            return await HandleRequestAsync(async() =>
            {
                using(var stream = file.OpenReadStream())
                {
                    await _eventService.SaveEventImageAsync(stream, id);
                }
            });
        }

        [Authorize, HttpPost("[action]/{id}")]
        public async Task<IActionResult> UploadEventTnc(IFormFile file, string id)
        {
            return await HandleRequestAsync(async() =>
            {
                using(var stream = file.OpenReadStream())
                {
                    await _eventService.SaveEventTncAsync(stream, id, file.FileName);
                }
            });
        }

        [Authorize, HttpPost("[action]/{id}")]
        public async Task<IActionResult> UploadPromoCodeList(IFormFile file, string id)
        {
            return await HandleRequestAsync(async() =>
            {
                using(var stream = file.OpenReadStream())
                {
                    await _eventService.SaveEventTncAsync(stream, id, file.FileName);
                }
            });
        }

        [Authorize, HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetPrice(string eventId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var user = await GetUserAsync();
                var price = _eventService.GetPriceAsync(Guid.Parse(eventId), user.Id);
                return price;
            });
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> CreateEvent()
        {
            return await HandleRequestWithDataAsync(async() => _eventService.CreateEvent((await GetUserAsync()).Id, GetFederationId().Value), false, true);
        }

        #endregion

        #region Private Methods

        private async Task<bool> CheckEventOwnerAsync(Guid eventId)
        {
            var user = await GetUserAsync();
            var eventOwner = await _eventService.GetEventOwnerIdAsync(eventId);
            return !string.IsNullOrEmpty(eventOwner) && user.Id == eventOwner;
        }

        #endregion  
    }
}