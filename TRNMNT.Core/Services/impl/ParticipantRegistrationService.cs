using System;
using System.Threading.Tasks;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantRegistrationService : IParticipantRegistrationService
    {
        #region Dependencies

        private readonly IPaymentService _paymentService;
        private readonly IParticipantService _participantService;
        private readonly IOrderService _orderService;
        private readonly IEventService _eventService;
        private readonly IAppDbContext _unitOfWork;
        private readonly IPromoCodeService _promoCodeService;

        #endregion

        #region .ctor

        public ParticipantRegistrationService(IOrderService orderService,
            IPaymentService paymentService,
            IParticipantService participantService,
            IEventService eventService,
            IPromoCodeService promoCodeService,
            IAppDbContext unitOfWork)
        {
            _orderService = orderService;
            _participantService = participantService;
            _paymentService = paymentService;
            _eventService = eventService;
            _unitOfWork = unitOfWork;
            _promoCodeService = promoCodeService;
        }

        #endregion

        #region Public Methods

        public async Task<ParticipantRegistrationResult> ProcessParticipantRegistrationAsync(Guid eventId, ParticipantRegistrationModel model, string callbackUrl)
        {
            if (await _participantService.IsParticipantExistsAsync(model, eventId))
            {
                return new ParticipantRegistrationResult
                {
                    Success = false,
                    Reason = DefaultMessage.ParticipantRegistrationParticipantIsAlreadyExists
                };
            }
            var participant = _participantService.CreatePaticipant(model, eventId);
            var promoCodeUsed = await _promoCodeService.ValidateCodeAsync(eventId, model.PromoCode, participant.ParticipantId.ToString());
            _participantService.AddParticipant(participant);
            var price = await _eventService.GetPriceAsync(eventId, promoCodeUsed);
            var order = _orderService.GetNewOrder(OrderTypeEnum.EventParticipation, price, "UAH", participant.ParticipantId.ToString());
            _orderService.AddOrder(order);
            var paymentData = _paymentService.GetPaymentDataModel(order, callbackUrl);
            await _unitOfWork.SaveAsync();

            return new ParticipantRegistrationResult
            {
                ParticipantId = participant.ParticipantId.ToString(),
                Success = true,
                PaymentData = paymentData
            };
        }

        #endregion
    }
}
