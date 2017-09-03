using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Data.Entities;
using TRNMNT.Data.UnitOfWork;
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
        private IUnitOfWork unitOfWork;

        public ParticipantRegistrationService(IOrderService orderService, 
            IPaymentService paymentService, 
            IParticipantService participantService, 
            IEventService eventService,
            IUnitOfWork unitOfWork)
        {
            this.orderService = orderService;
            this.participantService = participantService;
            this.paymentService = paymentService;
            this.eventService = eventService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ParticipantRegistrationResult> ProcessParticipantRegistrationAsync(ParticipantRegistrationModel model, string userId, string callbackUrl) {
            if (await participantService.IsParticipantExistsAsync(model))
            {
                return new ParticipantRegistrationResult() {
                    Success = false,
                    Reason = DefaultMessage.PARTICIPANT_REGISTRATION_PARTICIPANT_ALREADY_EXISTS
                };
            }
            else
            {
                var participant = GetParticipantByModel(model);

                var price = await eventService.GetPrice(model.EventId, userId);
                var order = orderService.GetNewOrder(OrderTypeEnum.EventParticipation, userId, price, "UAH", participant.ParticipantId.ToString());
                var paymentData = paymentService.GetPaymentDataModel(order, callbackUrl);

                await participantService.AddParticipant(participant, false);
                await orderService.AddOrderAsync(order, false);
                await unitOfWork.SaveAsync();

                return new ParticipantRegistrationResult() {
                    ParticipantId = participant.ParticipantId.ToString(),
                    Success = true,
                    PaymentData = paymentData
                };
            }

        }



        #region private helpers
        private Participant GetParticipantByModel(ParticipantRegistrationModel model)
        {
            return new Participant()
            {
                ParticipantId = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                TeamId = Guid.Parse(model.TeamId),
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CategoryId = Guid.Parse(model.CategoryId),
                WeightDivisionId = Guid.Parse(model.WeightDivisionId),
                EventId = model.EventId,
                UserId = model.UserId,
                IsActive = true,
                IsApproved = false,
                UpdateTS = DateTime.UtcNow,
            };
        }
#endregion
    }


}
