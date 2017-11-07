using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Data.Context;
using TRNMNT.Web.Core.Const;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public class ParticipantRegistrationService : IParticipantRegistrationService
    {
        private IPaymentService paymentService;
        private IParticipantService participantService;
        private IOrderService orderService;
        private IEventService eventService;
        private IAppDbContext unitOfWork;
        private IPromoCodeService promoCodeService;

        public ParticipantRegistrationService(IOrderService orderService,
            IPaymentService paymentService,
            IParticipantService participantService,
            IEventService eventService,
            IPromoCodeService promoCodeService,
            IAppDbContext unitOfWork)
        {
            this.orderService = orderService;
            this.participantService = participantService;
            this.paymentService = paymentService;
            this.eventService = eventService;
            this.unitOfWork = unitOfWork;
            this.promoCodeService = promoCodeService;
        }

        public async Task<ParticipantRegistrationResult> ProcessParticipantRegistrationAsync(Guid eventId, ParticipantRegistrationModel model, string callbackUrl)
        {
            if (await participantService.IsParticipantExistsAsync(model, eventId))
            {
                return new ParticipantRegistrationResult()
                {
                    Success = false,
                    Reason = DefaultMessage.PARTICIPANT_REGISTRATION_PARTICIPANT_ALREADY_EXISTS
                };
            }
            else
            {
                var participant = participantService.CreatePaticipant(model, eventId);
                var promoCodeUsed = await promoCodeService.ValidateCodeAsync(eventId, model.PromoCode, participant.ParticipantId.ToString());
                participantService.AddParticipant(participant);
                var price = await eventService.GetPrice(eventId, promoCodeUsed);
                var order = orderService.GetNewOrder(OrderTypeEnum.EventParticipation, price, "UAH", $"{participant.FirstName} {participant.LastName} {participant.ParticipantId.ToString()}");
                orderService.AddOrder(order);
                var paymentData = paymentService.GetPaymentDataModel(order, callbackUrl);
                await unitOfWork.SaveAsync();

                return new ParticipantRegistrationResult()
                {
                    ParticipantId = participant.ParticipantId.ToString(),
                    Success = true,
                    PaymentData = paymentData
                };
            }

        }
    }


}
