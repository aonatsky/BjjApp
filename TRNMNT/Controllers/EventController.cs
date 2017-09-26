using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Entities;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using TRNMNT.Data.Repositories;
using System.Linq;
using TRNMNT.Core.Model.Event;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class EventController : CRUDController<Event>
    {
        private IEventService eventService;
        IHttpContextAccessor httpContextAccessor;

        public EventController(IEventService eventService, ILogger<EventController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService, IRepository<Event> repository) : base(logger, repository, httpContextAccessor, userService)
        {
            this.eventService = eventService;
            this.httpContextAccessor = httpContextAccessor;

        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventModel eventModel)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                if (await CheckEventOwnerAsync(eventModel.EventId))
                {
                    await eventService.UpdateEventAsync(eventModel);
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }


        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> GetNewEvent()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var user = await GetUserAsync();
                var addedEvent = await eventService.GetNewEventAsync(user.Id);
                return Ok(JsonConvert.SerializeObject(addedEvent, jsonSerializerSettings));
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
                var eventModel = await eventService.GetFullEventAsync(id);
                if (eventModel != null)
                {
                    var jsonobj = JsonConvert.SerializeObject(eventModel, jsonSerializerSettings);
                    return Ok(jsonobj);
                }
                else
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }
                
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

                var events = await eventService.GetEventsForOwnerAsync((await GetUserAsync()).Id);
                return events.ToArray();

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
                return null;
            }
        }

        [AllowAnonymous, HttpGet("[action]/{url}")]
        public async Task<IActionResult> GetEventByUrl(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {

                var _event = await eventService.GetEventByPrefixAsync(url);
                var jsonobj = JsonConvert.SerializeObject(_event, jsonSerializerSettings);
                return Ok(jsonobj);

            }
            catch (Exception e)
            {

                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize, HttpGet("[action]")]
        public async Task IsPrefixExists(string prefix)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                if (await eventService.IsPrefixExistAsync(prefix))
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
                    await eventService.AddEventImageAsync(stream, id);
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
                    await eventService.SaveEventTncAsync(stream, id, file.FileName);

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
                    await eventService.SaveEventTncAsync(stream, id, file.FileName);
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
                var price = eventService.GetPrice(Guid.Parse(eventId), user.Id);
                return Ok(price);
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            }
        }


        public override IQueryable<Event> ModifyQuery(string key, string value, IQueryable<Event> query)
        {
            throw new NotImplementedException();
        }

        #region helpers
        private async Task<bool> CheckEventOwnerAsync(Guid eventId)
        {
            var user = await GetUserAsync();
            var eventOwner = await eventService.GetEventOwnerIdAsync(eventId);
            if (eventOwner != "" && user.Id == eventOwner)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
        #endregion
    }
}
