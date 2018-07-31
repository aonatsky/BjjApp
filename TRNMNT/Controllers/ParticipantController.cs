using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.FileProcessingOptions;
using TRNMNT.Core.Model.Participant;
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
                var eventId = GetEventId();
                if (eventId != null)
                {
                    var user = await GetUserAsync();
                    var callbackUrl = Url.Action("ConfirmPayment", "Payment", null, "http");
                    var redirectUrl = $"{Request.Host}/event/event-registration-complete";
                    var result = await _participantService.ProcessParticipantRegistrationAsync(eventId.Value, model, callbackUrl, redirectUrl, await GetUserAsync());
                    return Success(result);
                }
                return (null, HttpStatusCode.NotFound);
            });
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

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> ParticipantsTable([FromBody] ParticipantFilterModel filter)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                var participants = await _participantService.GetFilteredParticipantsAsync(filter);
                return Success(participants);
            });
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> ParticipantsDropdownData(Guid eventId)
        {

            return await HandleRequestWithDataAsync(async() =>
            {
                var teams = await _teamService.GetTeamsAsync(GetFederationId().Value);
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

        [Authorize, HttpPost("[action]/{eventId}")]
        public async Task<IActionResult> UploadParticipantsFromFile(IFormFile file, Guid eventId)
        {
            return await HandleRequestAsync(async() =>
            {
                var options = new ParticipantListProcessingOptions
                {
                EventId = eventId,
                FederationId = GetFederationId().Value
                };
                await _fileProcessiongService.ProcessFileAsync(file, options);
            });
        }

        [Authorize, HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] ParticipantTableModel participantModel)
        {
            return await HandleRequestWithDataAsync(async() =>
            {
                return await _participantService.UpdateParticipantAsync(participantModel);
            });
        }

        [Authorize, HttpDelete("[action]/{participantId}")]
        public async Task<IActionResult> Delete(Guid participantId)
        {
            return await HandleRequestAsync(async() =>
            {
                await _participantService.DeleteParticipantAsync(participantId);
            });
        }

        #endregion
    }
}