using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.Context;

namespace TRNMNT.Core.Services
{
    public class ParticipantService : IParticipantService
    {
        private IRepository<Participant> repository;
        private ITeamService teamService;
        private IOrderService orderService;
        private IPaymentService paymentService;
        private IAppDbContext unitOfWork;

        public ParticipantService(IRepository<Participant> repository, ITeamService teamService, IOrderService orderService, IPaymentService paymentService,
            IAppDbContext unitOfWork)
        {
            this.repository = repository;
            this.teamService = teamService;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.unitOfWork = unitOfWork;
        }



        public async Task<bool> IsParticipantExistsAsync(ParticipantModelBase model, Guid eventId)
        {
            return await repository.GetAll().AnyAsync(p =>
             p.EventId == eventId
             && p.FirstName == model.FirstName
             && p.LastName == model.LastName
             && p.DateOfBirth == model.DateOfBirth
             );
        }

        public void AddParticipant(Participant participant)
        {
            repository.Add(participant);
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
        
    }


}







