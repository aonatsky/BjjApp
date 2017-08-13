using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
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
        private IParticipantService participantService;
        private ITeamService teamService;

        public ParticipantService(IRepository<Participant> repository, ITeamService teamService)
        {
            this.repository = repository;
            this.teamService = teamService;
        }

        public async Task<bool> IsParticipantExistsAsync(Participant participant)
        {
            return await repository.GetAll().AnyAsync(p =>
             p.EventId == participant.EventId
             && p.FirstName == participant.FirstName
             && p.LastName == participant.LastName
             && p.DateOfBirth == participant.DateOfBirth
             );
        }

        public async Task<ParticipantRegistrationResult> RegisterParticipantAsync(ParticipantRegistrationModel model)
        {
            var participant = await GetParticipantByModel(model);
            if (await IsParticipantExistsAsync(participant))
            {
                return new ParticipantRegistrationResult(false, DefaultMessage.PARTICIPANT_REGISTRATION_PARTICIPANT_ALREADY_EXISTS);
            }
            else
            {
                repository.Add(participant);
                await repository.SaveAsync();
                return new ParticipantRegistrationResult(true);
            }
        }

        private async Task<Participant> GetParticipantByModel(ParticipantRegistrationModel model)
        {
            var team = await GetTeamAsync(model.Team);
            return new Participant()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Team = team,
                TeamId = team.TeamId,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CategoryId = Guid.Parse(model.CategoryId),
                WeightDivisionId = Guid.Parse(model.WeightDivisionId),
                EventId = model.EventId,
                UserId = model.UserId,
                IsActive = true,
                IsApproved = false,
                UpdateTS = DateTime.UtcNow
            };
        }

        private async Task<Team> GetTeamAsync(string name)
        {
            var team = await teamService.GetTeamByNameAsync(name);
            if (team == null)
            {
                team = new Team()
                {
                    TeamId = Guid.NewGuid(),
                    Name = name
                };
            }
            return team;
        }
    }


}







