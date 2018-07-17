using System;
using System.Threading.Tasks;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantRegistrationService : IParticipantRegistrationService
    {
        #region Dependencies

        private readonly IPaymentService _paymentService;
        private readonly IParticipantService _participantService;
        private readonly IOrderService _orderService;
        private readonly IEventService _eventService;
        private readonly IFederationMembershipService _federationMembershipService;

        #endregion

        #region .ctor

        public ParticipantRegistrationService(IOrderService orderService,
            IPaymentService paymentService,
            IParticipantService participantService,
            IEventService eventService,
            IPromoCodeService promoCodeService,
            IAppDbContext unitOfWork,
            IFederationMembershipService federationMembershipService)
        {
            _orderService = orderService;
            _participantService = participantService;
            _paymentService = paymentService;
            _eventService = eventService;
            _federationMembershipService = federationMembershipService;
        }

        #endregion

        #region Public Methods

        public async Task<ParticipantRegistrationResult> ProcessParticipantRegistrationAsync(Guid eventId, ParticipantRegistrationModel model, string callbackUrl, User user)
        {
            if (await _participantService.IsParticipantExistsAsync(model, eventId))
            {
                return new ParticipantRegistrationResult
                {
                    Success = false,
                        Reason = DefaultMessage.ParticipantRegistrationParticipantIsAlreadyExists
                };
            }
            var participantId = _participantService.AddParticipant(model, eventId);
            var order = _orderService.AddNewOrder(OrderTypeEnum.EventParticipation, await _eventService.GetPriceAsync(eventId, user.Id), "UAH", participantId.ToString(), user.Id);
            var paymentData = _paymentService.GetPaymentDataModel(order, callbackUrl);

            return new ParticipantRegistrationResult
            {
                ParticipantId = participantId.ToString(),
                    Success = true,
                    PaymentData = paymentData
            };
        }

        #endregion
    }
}