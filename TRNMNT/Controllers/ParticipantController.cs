using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IParticipantRegistrationService _participantRegistrationService;
        private readonly ITeamService _teamService;
        private readonly IWeightDivisionService _weightDivisionService;
        private readonly ICategoryService _categoryService;
        private readonly IFileProcessiongService<ParticipantListProcessingOptions> _fileProcessiongService;

        #endregion

        #region .ctor
        public ParticipantController(IEventService eventService,
            ILogger<TeamController> logger,
            IUserService userService,
            IParticipantService participantService,
            IPaymentService paymentService,
            IOrderService orderService,
            IParticipantRegistrationService participantRegistrationService,
            ITeamService teamService,
            IWeightDivisionService weightDivisionService,
            ICategoryService categoryService,
            IFileProcessiongService<ParticipantListProcessingOptions> fileProcessiongService,
            IAppDbContext context)
            : base(logger, userService, eventService, context)
        {

            _participantService = participantService;
            _participantRegistrationService = participantRegistrationService;
            _teamService = teamService;
            _weightDivisionService = weightDivisionService;
            _categoryService = categoryService;
            _fileProcessiongService = fileProcessiongService;
        }

        #endregion

        #region Public Methods

        [Authorize, HttpPost("[action]")]
        public async Task<IActionResult> IsParticipantExist([FromBody]ParticipantRegistrationModel model)
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
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ProcessParticipantRegistration([FromBody]ParticipantRegistrationModel model)
        {
            try
            {
                var eventId = GetEventId();
                if (eventId != null)
                {
                    var user = await GetUserAsync();
                    var callbackUrl = $"{Request.Host}{Url.Action("ConfirmPayment", "Payment")}";
                    var result = await _participantRegistrationService.ProcessParticipantRegistrationAsync(eventId.Value, model, callbackUrl);
                    return Ok(JsonConvert.SerializeObject(result, JsonSerializerSettings));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpPost("[action]/{eventId}")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentDataModel model)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                HandleException(e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> ParticipantsTable(ParticipantFilterModel filter)
        {
            return await HandleRequestWithDataAsync(async () =>
            {
                var participants = await _participantService.GetFilteredParticipantsAsync(filter);
                return Success(participants);
            });
        }

        [Authorize, HttpGet("[action]")]
        public async Task<IActionResult> ParticipantsDropdownData(Guid eventId)
        {

            return await HandleRequestWithDataAsync(async () =>
            {
                var teams = await _teamService.GetTeamsAsync(eventId);
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
            return await HandleRequestWithDataAsync(async () =>
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
            return await HandleRequestWithDataAsync(async () =>
            {
                return await _participantService.UpdateParticipantAsync(participantModel);
            });
        }

        [Authorize, HttpDelete("[action]/{participantId}")]
        public async Task<IActionResult> Delete(Guid participantId)
        {
            return await HandleRequestAsync(async () =>
            {
                await _participantService.DeleteParticipantAsync(participantId);
            });
        }

        #endregion
    }
}

