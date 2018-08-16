using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Team;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantProcessingService : IParticipantProcessingService
    {
        #region Dependencies

        private readonly IRepository<Participant> _participantRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<WeightDivision> _weightDivisionRepository;


        #endregion

        #region .ctor

        public ParticipantProcessingService(
            IRepository<Team> teamRepository,
            IRepository<Category> categoryRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Participant> participantRepository)

        {
            _teamRepository = teamRepository;
            _categoryRepository = categoryRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _participantRepository = participantRepository;
        }

        #endregion
        
        #region Public Methods

        public async Task<List<string>> AddParticipantsByModelsAsync(List<ParticipantModel> particitantModels, Guid eventId, Guid federationId)
        {
            var messageList = new List<string>();
            var existingTeamsBase = await GetExistingTeams(federationId);
            var eventCategories = await _categoryRepository.GetAll(w => w.EventId == eventId).ToListAsync();
            var eventWeightDivisions =
                await _weightDivisionRepository.GetAll(w => w.Category.EventId == eventId && !w.IsAbsolute).ToListAsync();
            var existingParticipants = await _participantRepository.GetAll().ToListAsync();

            var participantsToAdd = new List<Participant>();
            var teamsToAdd = new List<Team>();
            var comparer = new ParticipantComparer();
            var index = 0;
            foreach (var model in particitantModels)
            {
                index++;
                if (!IsParticipantModelValid(model, messageList, index, eventCategories, eventWeightDivisions))
                {
                    continue;
                }
                bool.TryParse(model.IsMember, out var isMember);
                var participantId = Guid.NewGuid();
                var category = eventCategories.FirstOrDefault(c => c.Name == model.Category);
                var participant = new Participant
                {
                    ParticipantId = participantId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = DateTime.Parse(model.DateOfBirth),
                    EventId = eventId,
                    TeamId = ProcessTeam(model.Team, existingTeamsBase, teamsToAdd),
                    WeightDivisionId = eventWeightDivisions.First(wd => wd.Name.Equals(model.WeightDivision, StringComparison.OrdinalIgnoreCase) && wd.CategoryId == category.CategoryId).WeightDivisionId,
                    CategoryId = eventCategories.First(c => c.Name.Equals(model.Category, StringComparison.OrdinalIgnoreCase)).CategoryId,
                    IsMember = isMember,
                    IsActive = true,
                    WeightInStatus = ApprovalStatus.Approved,
                    ApprovalStatus = ApprovalStatus.Approved
                };

                if (!existingParticipants.Contains(participant, comparer) && !participantsToAdd.Contains(participant, comparer))
                {
                    participantsToAdd.Add(participant);
                }
            }
            teamsToAdd.ForEach(t => t.FederationId = federationId);
            _teamRepository.AddRange(teamsToAdd);
            _participantRepository.AddRange(participantsToAdd);
            return messageList;
        }
        
        #endregion
        
        #region Private methods

        private bool IsParticipantModelValid(ParticipantModel model, List<string> messageBuilder, int modelIndex, List<Category> categories, List<WeightDivision> weightDivisions)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(model.FirstName))
            {
                messageBuilder.Add($"First Name for Particitant with index {modelIndex} is invalid");
                isValid = false;
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                messageBuilder.Add($"Last Name for Particitant with index {modelIndex}  is invalid");
                isValid = false;
            }
            var nValid = !string.IsNullOrEmpty(model.FirstName) && !string.IsNullOrEmpty(model.LastName);
            var identifier = nValid ? $"{model.FirstName} {model.LastName}" : $"Particitant with index {modelIndex}";

            if (!DateTime.TryParse(model.DateOfBirth, out _))
            {
                messageBuilder.Add($"Date of birth for '{identifier}' is invalid");
                isValid = false;
            }
            if (string.IsNullOrEmpty(model.Team))
            {
                messageBuilder.Add($"Team name for '{identifier}' is invalid");
                isValid = false;
            }
            var weightDivision = weightDivisions.FirstOrDefault(w => w.Name.Equals(model.WeightDivision, StringComparison.OrdinalIgnoreCase));
            if (weightDivision == null)
            {
                messageBuilder.Add($"Weight division '{model.WeightDivision}' for '{identifier}' is invalid");
                isValid = false;
            }
            var category = categories.FirstOrDefault(c => c.Name.Equals(model.Category, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                messageBuilder.Add($"Category '{model.Category}' for '{identifier}' is invalid");
                isValid = false;
            }
            return isValid;
        }


        private async Task<List<TeamModelBase>> GetExistingTeams(Guid federationId)
        {
            return await _teamRepository.GetAll(t => t.FederationId == federationId).Select(t => new TeamModelBase
            {
                Name = t.Name,
                TeamId = t.TeamId.ToString()
            }).ToListAsync();
        }

        #endregion

        #region Models parsing

        private Guid ProcessTeam(string name, IEnumerable<TeamModelBase> existingTeamNames, List<Team> teamsToAdd)
        {
            var team = teamsToAdd.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (team == null)
            {
                var teamBase = existingTeamNames.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (teamBase == null)
                {
                    team = new Team
                    {
                        TeamId = Guid.NewGuid(),
                        Name = name,
                        ApprovalStatus = ApprovalStatus.Approved,
                        FederationApprovalStatus = ApprovalStatus.Pending
                    };
                    teamsToAdd.Add(team);
                }
                else
                {
                    return new Guid(teamBase.TeamId);
                }
            }
            return team.TeamId;

        }

        #endregion

        private class ParticipantComparer : IEqualityComparer<Participant>
        {
            public bool Equals(Participant x, Participant y)
            {
                return x.FirstName == y.FirstName && x.LastName == y.LastName && x.DateOfBirth == y.DateOfBirth;
            }

            public int GetHashCode(Participant fighter)
            {
                return fighter.FirstName.GetHashCode() ^ fighter.LastName.GetHashCode() ^ fighter.DateOfBirth.GetHashCode();
            }
        }
    }
}
