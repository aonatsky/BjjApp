using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Const;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.FileProcessingOptions;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Shared;
using TRNMNT.Core.Services;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class ParticipantController : BaseController
    {
        #region Dependencies

        private readonly IParticipantService _participantService;
        private readonly ITeamService _teamService;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly ICategoryService _categoryService;
        private readonly IFederationMembershipService _federationMembershipService;
        private readonly IFileProcessiongService<ParticipantListProcessingOptions> _fileProcessiongService;

        #endregion

        #region .ctor
        public ParticipantController(IEventService eventService,
            ILogger<TeamController> logger,
            IUserService userService,
            IParticipantService participantService,
            IPaymentService paymentService,
            IOrderService orderService,
            ITeamService teamService,
            IWeightDivisionService weightDivisionService,
            ICategoryService categoryService,
            IFederationMembershipService federationMembershipService,
            IFileProcessiongService<ParticipantListProcessingOptions> fileProcessiongService,
            IConfiguration configuration,
            IAppDbContext context) : base(logger, userService, eventService, context, configuration)
        {
            _participantService = participantService;
            _teamService = teamService;
            _weightDivisionService = weightDivisionService;
            _categoryService = categoryService;
            _federationMembershipService = federationMembershipService;
            _fileProcessiongService = fileProcessiongService;
        }

        #endregion

        #region Public Methods

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> IsParticipantExist([FromBody] ParticipantRegistrationModel model)
        {
            try
            {
                if (GetEventId() != null)
                {
                    var result = await _participantService.IsParticipantExistsAsync(model, GetEventId().Value);
                    return Ok(JsonConvert.SerializeObject(result, JsonSerializerSettings));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ProcessParticipantRegistration([FromBody] ParticipantRegistrationModel model)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var callbackUrl = Url.Action("ConfirmPayment", "Payment", null, "http");
                var redirectUrl = $"{Request.Host}/participant/my-events";
                var result = await _participantService.ProcessParticipantRegistrationAsync(GetEventId().Value, GetFederationId().Value, model, callbackUrl, redirectUrl, await GetUserAsync());
                return Success(result);
            }, true, true);
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ProcessParticipantTeamRegistration([FromBody] List<ParticipantRegistrationModel> models)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var callbackUrl = Url.Action("ConfirmPayment", "Payment", null, "http");
                var redirectUrl = $"{Request.Host}/participant/my-events";
                var result = await _participantService.ProcessParticipantTeamRegistrationAsync(GetEventId().Value, GetFederationId().Value, models, callbackUrl, redirectUrl, await GetUserAsync());
                return Success(result);
            }, true, true);
        }

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ProcessMembershipRegistration()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var eventId = GetEventId();
                if (eventId != null)
                {
                    var user = await GetUserAsync();
                    var callbackUrl = Url.Action("ConfirmPayment", "Payment", null, "http");
                    var redirectUrl = $"{Request.Host}/event/membership-registration-complete";
                    var result = await _federationMembershipService.ProcessFederationMembershipAsync(GetFederationId().Value, callbackUrl, redirectUrl, await GetUserAsync());
                    return Success(result);
                }
                return (null, HttpStatusCode.NotFound);
            });
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> GetUserParticipations()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var user = await GetUserAsync();
                var roles = await UserService.GetUserRolesAsync(user);
                var participants = await _participantService.GetUserParticipantsAsync(user, roles.Contains(Roles.TeamOwner));
                return Success(participants);
            });
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<IActionResult> ParticipantsTable([FromBody] ParticipantFilterModel filter)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var participants = await _participantService.GetFilteredParticipantsAsync(filter);
                return Success(participants);
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ParticipantsDropdownData(Guid eventId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var teams = await _teamService.GetTeamsForEventAsync(GetFederationId().Value);
                var categories = await _categoryService.GetCategoriesByEventIdAsync(eventId);
                var weightDivisions = await _weightDivisionService.GetWeightDivisionModelsByEventIdAsync(eventId, false);
                return Success(new ParticipantDdlModel
                {
                    Teams = teams,
                        Categories = categories,
                        WeightDivisions = weightDivisions
                });
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]/{eventId}")]
        public async Task<IActionResult> UploadParticipantsFromFile(IFormFile file, Guid eventId)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var options = new ParticipantListProcessingOptions
                {
                    EventId = eventId,
                        FederationId = GetFederationId().Value
                };
                return await _fileProcessiongService.ProcessFileAsync(file, options);
            });
        }

        [Authorize, HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] ParticipantTableModel participantModel)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var user = await GetUserAsync();
                var roles = await UserService.GetUserRolesAsync(user);
                if (roles.Contains(Roles.TeamOwner))
                {
                    var team = await _teamService.GetTeamForOwnerAsync(user.Id);
                    if (participantModel.TeamId == team.TeamId)
                    {
                        return Success(await _participantService.UpdateParticipantAsync(participantModel));
                    }
                }
                if (roles.Contains(Roles.Participant))
                {
                    if (participantModel.UserId == user.Id)
                    {
                        return Success(await _participantService.UpdateParticipantAsync(participantModel));
                    }
                }
                if (roles.Contains(Roles.Admin) || roles.Contains(Roles.Owner) || roles.Contains(Roles.FederationOwner))
                {
                    return Success(await _participantService.UpdateParticipantAsync(participantModel));
                }
                return UnauthorizedResponse();
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpDelete("[action]/{participantId}")]
        public async Task<IActionResult> Delete(Guid participantId)
        {
            return await HandleRequestAsync(async() =>
            {
                await _participantService.DeleteParticipantAsync(participantId);
            });
        }

        [Authorize(Roles = "FederationOwner, Owner, Admin"), HttpPost("[action]/{participantId}")]
        public async Task<IActionResult> SetWeightInStatus(Guid participantId, [FromBody] SimpleStringModel statusModel)
        {
            return await HandleRequestAsync(async() =>
            {
                await _participantService.SetWeightInStatus(participantId, statusModel.data);
            });
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> IsFederationMember()
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                return await _federationMembershipService.IsFederationMemberAsync(GetFederationId().Value, (await GetUserAsync()).Id);
            }, false, true);
        }

        #endregion
    }
}