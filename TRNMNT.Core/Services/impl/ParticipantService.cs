using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantService : IParticipantService
    {
        #region Dependencies

        private readonly IRepository<Participant> _repository;
        private readonly ITeamService _teamService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IAppDbContext _unitOfWork;

        #endregion

        #region .ctor

        public ParticipantService(IRepository<Participant> repository, ITeamService teamService, IOrderService orderService, IPaymentService paymentService,
            IAppDbContext unitOfWork)
        {
            _repository = repository;
            _teamService = teamService;
            _orderService = orderService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public async Task<bool> IsParticipantExistsAsync(ParticipantModelBase model, Guid eventId)
        {
            return await _repository.GetAll().AnyAsync(p =>
             p.EventId == eventId
             && p.FirstName == model.FirstName
             && p.LastName == model.LastName
             && p.DateOfBirth == model.DateOfBirth);
        }

        public void AddParticipant(Participant participant)
        {
            _repository.Add(participant);
        }

        public Participant CreatePaticipant(ParticipantRegistrationModel model, Guid eventId)
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
                EventId = eventId,
                UserId = model.UserId,
                IsActive = true,
                IsApproved = false,
                UpdateTS = DateTime.UtcNow,
            };
        }

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var participant = await _repository.GetByIDAsync(entityId);
            if (participant != null)
            {
                participant.IsApproved = true;
                participant.OrderId = orderId;
                _repository.Update(participant);
            }
        }

        #endregion
    }
}