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
using TRNMNT.Data.UnitOfWork;

namespace TRNMNT.Core.Services
{
    public class ParticipantService : IParticipantService
    {
        private IRepository<Participant> repository;
        private ITeamService teamService;
        private IOrderService orderService;
        private IPaymentService paymentService;
        private IUnitOfWork unitOfWork;

        public ParticipantService(IRepository<Participant> repository, ITeamService teamService, IOrderService orderService, IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.teamService = teamService;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.unitOfWork = unitOfWork;
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

        public async Task AddParticipant(Participant participant, bool saveContext = true)
        {
            repository.Add(participant);
            if (saveContext)
            {
                await unitOfWork.SaveAsync();
            }
        }

       
    }
    

}







