using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Web.Core.Const;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public class ParticipantService : IParticipantService
    {
        private IRepository<Participant> repository;
        private ITeamService teamService;
        private IOrderService orderService;
        private IPaymentService paymentService;

        public ParticipantService(IRepository<Participant> repository, ITeamService teamService, IOrderService orderService, IPaymentService paymentService)
        {
            this.repository = repository;
            this.teamService = teamService;
            this.orderService = orderService;
            this.paymentService = paymentService;
        }



        public async Task<bool> IsParticipantExistsAsync(ParticipantModelBase model)
        {
            return await repository.GetAll().AnyAsync(p =>
             p.EventId == model.EventId
             && p.FirstName == model.FirstName
             && p.LastName == model.LastName
             && p.DateOfBirth == model.DateOfBirth
             );
        }

        public async Task AddParticipant(Participant participant)
        {
            repository.Add(participant);
            await repository.SaveAsync();
        }

       
    }
    

}







