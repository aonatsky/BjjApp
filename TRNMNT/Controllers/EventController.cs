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


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class EventController : BaseController
    {
        private IEventService eventService;
        IHttpContextAccessor httpContextAccessor;

        public EventController(IEventService eventService, ILogger<EventController> logger, IHttpContextAccessor httpContextAccessor, IUserService userService) : base(logger, httpContextAccessor, userService)
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
                await eventService.SaveEventAsync(eventToAdd,user.Id);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
            }
        }


        [Authorize, HttpGet("[action]/{id}")]
        public async Task GetEvent(Guid id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var _event = await eventService.GetFullEventAsync(id);
                await Response.WriteAsync(JsonConvert.SerializeObject(_event));

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);

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

    }
}
