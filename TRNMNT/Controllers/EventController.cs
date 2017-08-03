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
using Newtonsoft.Json.Serialization;
using TRNMNT.Data.Repositories;
using System.Linq;


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
        public async Task SaveEvent([FromBody] Event eventToAdd)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var user = await GetUserAsync();
                await eventService.SaveEventAsync(eventToAdd, user.Id);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
            }
        }


        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var _event = await eventService.GetFullEventAsync(id);
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore // ignore null values
                };
                await Response.WriteAsync(JsonConvert.SerializeObject(_event, jsonSerializerSettings));
                return Ok();
            }
            catch (Exception e)
            {
                HandleException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [Authorize, HttpGet("[action]")]
        public async Task<Event[]> GetEventsForOwner()
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


        [Authorize, HttpGet("[action]/{url}")]
        public async Task<string> GetEventIdByUrl(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {

                var eventId= await eventService.GetEventIdAsync(url);
                return eventId;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
                return null;
            }
        }

        [AllowAnonymous, HttpGet("[action]/{url}")]
        public async Task<Event> GetEventByUrl(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {

                var _event = await eventService.GetEventByPrefixAsync(url);
                return _event;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
                return null;
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
        public async Task UploadEventImage(IFormFile file,string id)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await eventService.AddEventImageAsync(stream, id);
                }
                
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(ex);
            };
        }

        [Authorize, HttpPost("[action]/{id}")]
        public async Task UploadEventTnc(IFormFile file, string id)
        {
            try
            {
                await eventService.SaveEventTncAsync(file.OpenReadStream(), id, file.FileName);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(ex);
            };
        }


        [Authorize, HttpGet("[action]")]
        public async Task<string> CreateEvent()
        {
            try
            {
                return (await eventService.CreateEventAsync()).EventId.ToString();
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(ex);
                return "";
            };
        }

        public override IQueryable<Event> ProcessQuery(string key, string value, IQueryable<Event> query)
        {
            throw new NotImplementedException();
        }
    }
}
