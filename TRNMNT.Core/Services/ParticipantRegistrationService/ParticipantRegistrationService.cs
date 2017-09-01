using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Data.Entities;
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

        public ParticipantRegistrationService(IOrderService orderService, IPaymentService paymentService, IParticipantService participantservice, IEventService eventService)
        {
            this.orderService = orderService;
            this.participantService = participantservice;
            this.paymentService = paymentService;
            this.eventService = eventService;
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
                var participantId = participantService.AddParticipant(participant);
                var price = await eventService.GetPrice(model.EventId, userId);
                var order = await orderService.GetNewOrderAsync(OrderTypeEnum.EventParticipation, userId, price, "UAH", participantId.ToString());
                var paymentData = paymentService.GetPaymentDataModel(order, callbackUrl);
                return new ParticipantRegistrationResult() {
                    ParticipantId = participantId.ToString(),
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
