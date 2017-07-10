using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Entities;
using TRNMNT.Core.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class EventController : BaseController
    {
        private IEventService eventService;
        IHttpContextAccessor httpContextAccessor;

        public EventController(IEventService eventService, ILogger<EventController> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
        {
            this.eventService = eventService;
            this.httpContextAccessor = httpContextAccessor;
        }


        [Authorize, HttpPost("[action]")]
        public async Task AddEvent([FromBody] Event eventToAdd)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                await eventService.AddEventAsync(eventToAdd);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                HandleException(e);
            }
        }


        [Authorize, HttpGet("[action]")]
        public async Task<Event> GetEvent([FromBody] Guid id)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            try
            {
                var _event = await eventService.GetEventAsync(id);
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

    }
}
