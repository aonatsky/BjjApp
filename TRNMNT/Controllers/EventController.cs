using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Shared;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EventController : BaseController
    {
        #region Dependencies

        private readonly IEventService _eventService;

        #endregion

        #region .ctor

        public EventController(IEventService eventService, ILogger<EventController> logger, IUserService userService, IAppDbContext context, IConfiguration configuration) : base(logger, userService, eventService, context, configuration)
        {
            _eventService = eventService;
        }

        #endregion

        #region Public Methods

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventModelFull eventModel)
        {
            return await HandleRequestAsync(async() =>
            {
                await _eventService.UpdateEventAsync(eventModel);
                return HttpStatusCode.OK;
            });
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var eventModel = await _eventService.GetFullEventModelAsync(id);
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
                var eventModel = await _eventService.GetFullEventModelAsync(id);
                if (eventModel != null)
                {
                    return Success(eventModel);
                }
                return NotFoundResponse();
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpGet("[action]")]
        public async Task<IActionResult> GetEventsForOwner()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var user = await GetUserAsync();
                var events = await _eventService.GetEventsForOwnerAsync(user.Id, (await UserService.GetUserRolesAsync(user)).FirstOrDefault());
                return Success(events.ToArray());
            });
        }

        [AllowAnonymous, HttpGet("[action]")]
        public async Task<IActionResult> GetEventInfo()
        {
            return await HandleRequestWithDataAsync(async() =>(await _eventService.GetEventInfoAsync(GetEventId()), HttpStatusCode.OK), true);
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> IsPrefixExists(Guid id, [FromQuery(Name = "prefix")] string prefix)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                return await _eventService.IsEventUrlPrefixExistAsync(id, prefix);
            });
        }

        [Authorize, HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetEventDashboardData(Guid id)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                return await _eventService.GetEventDashboardDataAsync(id);
            });
        }

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpPost("[action]/{id}")]
        public async Task<IActionResult> SetCorrectionsEnabled(Guid id, [FromBody] SimpleModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _eventService.SetCorrectionsEnabledAsync(id, (bool) model.Data);
            });
        }

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpPost("[action]/{id}")]
        public async Task<IActionResult> SetParticipantListsPublish(Guid id, [FromBody] SimpleModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _eventService.SetParticipantListsPublishAsync(id, (bool) model.Data);
            });
        }

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpPost("[action]/{id}")]
        public async Task<IActionResult> SetBracketsPublish(Guid id, [FromBody] SimpleModel model)
        {
            return await HandleRequestAsync(async() =>
            {
                await _eventService.SetBracketsPublishAsync(id, (bool) model.Data);
            });
        }

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpPost("[action]/{id}")]
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

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpPost("[action]/{id}")]
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

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpPost("[action]/{id}")]
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
        public async Task<IActionResult> GetPrice(string eventId, [FromQuery(Name = "includeMembership")] bool includeMembership)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var user = await GetUserAsync();
                var price = _eventService.GetPriceAsync(Guid.Parse(eventId), user.Id, includeMembership);
                return price;
            });
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> GetTeamPrice([FromBody] List<ParticipantRegistrationModel> participants)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var price = await _eventService.GetTeamPriceAsync(GetEventId().Value, participants);
                return price;
            }, true, true);
        }

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpGet("[action]")]
        public async Task<IActionResult> CreateEvent()
        {
            return await HandleRequestWithDataAsync(async() => _eventService.CreateEvent((await GetUserAsync()).Id, GetFederationId().Value), false, true);
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> GetPrice([FromQuery(Name = "includeMembership")] bool includeMembership)
        {
            return await HandleRequestWithDataAsync(async() => await _eventService.GetPriceAsync(GetEventId().Value, (await GetUserAsync()).Id, includeMembership),
                true, true);
        }

        [Authorize(Roles = "Admin, FederationOwner, Owner"), HttpDelete("[action]/{eventId}")]
        public async Task<IActionResult> DeleteEvent(Guid eventId)
        {
            return await HandleRequestAsync(async() => await _eventService.DeleteEventAsync(eventId));
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