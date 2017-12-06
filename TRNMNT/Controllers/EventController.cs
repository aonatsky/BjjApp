using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Services;
using TRNMNT.Data.Context;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
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
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                if (await CheckEventOwnerAsync(eventModel.EventId))
                {
                    await _eventService.UpdateEventAsync(eventModel);
                    return Ok();
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var eventModel = await _eventService.GetFullEventAsync(id);
                if (eventModel != null)
                {
                    var jsonobj = JsonConvert.SerializeObject(eventModel, JsonSerializerSettings);
                    return Ok(jsonobj);
                }
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEventBaseInfo(Guid id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var eventModel = await _eventService.GetFullEventAsync(id);
                if (eventModel != null)
                {
                    var jsonobj = JsonConvert.SerializeObject(eventModel, JsonSerializerSettings);
                    return Ok(jsonobj);
                }
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [Authorize, HttpGet("[action]")]
        public async Task<EventModelBase[]> GetEventsForOwner()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {

                var events = await _eventService.GetEventsForOwnerAsync((await GetUserAsync()).Id);
                return events.ToArray();

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
                return null;
            }
        }

        [AllowAnonymous, HttpGet("[action]")]
        public async Task<IActionResult> GetEventInfo()
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                return JsonConvert.SerializeObject(await _eventService.GetEventInfoAsync(GetEventId().Value), JsonSerializerSettings);
                //return Ok(jsonobj);
            }, true);
        }

        [Authorize, HttpGet("[action]")]
        public async Task IsPrefixExists(string prefix)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                if (await _eventService.IsEventUrlPrefixExist(prefix))
                {
                    Response.StatusCode = (int)HttpStatusCode.Found;
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }


            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
            }
        }


        [Authorize, HttpPost("[action]/{id}")]
        public async Task<IActionResult> UploadEventImage(IFormFile file, string id)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await _eventService.SaveEventImageAsync(stream, id);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            };
        }

        [Authorize, HttpPost("[action]/{id}")]
        public async Task<IActionResult> UploadEventTnc(IFormFile file, string id)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await _eventService.SaveEventTncAsync(stream, id, file.FileName);
                }
                return Ok();
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            };
        }

        [Authorize, HttpPost("[action]/{id}")]
        public async Task<IActionResult> UploadPromoCodeList(IFormFile file, string id)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await _eventService.SaveEventTncAsync(stream, id, file.FileName);
                }
                return Ok();
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            };
        }

        [Authorize, HttpGet("[action]/{eventId}")]
        public async Task<IActionResult> GetPrice(string eventId)
        {
            try
            {
                var user = await GetUserAsync();
                var price = _eventService.GetPrice(Guid.Parse(eventId), user.Id);
                return Ok(price);
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> CreateEvent()
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                return JsonConvert.SerializeObject(_eventService.CreateEvent((await GetUserAsync()).Id, GetFederationId().Value), JsonSerializerSettings);
            }, false, true);
        }


        #endregion
        
        #region Private Methods

        private async Task<bool> CheckEventOwnerAsync(Guid eventId)
        {
            var user = await GetUserAsync();
            var eventOwner = await _eventService.GetEventOwnerIdAsync(eventId);
            if (!string.IsNullOrEmpty(eventOwner) && user.Id == eventOwner)
            {
                return true;
            }
            return false;
        }

        #endregion  
    }
}
