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

        public ParticipantService(IRepository<Participant> repository, ITeamService teamService)
        {
            this.repository = repository;
            this.teamService = teamService;
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

        public async Task<ParticipantRegistrationResult> RegisterParticipantAsync(ParticipantRegistrationModel model)
        {
            if (await IsParticipantExistsAsync(model))
            {
                return new ParticipantRegistrationResult(false, DefaultMessage.PARTICIPANT_REGISTRATION_PARTICIPANT_ALREADY_EXISTS);
            }
            else
            {
                var participant = GetParticipantByModel(model);
                repository.Add(participant);
                await repository.SaveAsync();
                return new ParticipantRegistrationResult(true);
            }
        }

        #region private helpers
        private  Participant GetParticipantByModel(ParticipantRegistrationModel model)
        {
            return new Participant()
            {
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
    } 
    #endregion

}







