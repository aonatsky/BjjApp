using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
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

        public List<ParticipantModel> GetParticitantModelsByFilter(ParticitantFilterModel filter)
        {
            var fighters = GetParticitantByFilter(filter);
            return GetModels(fighters);
        }

        public async Task<List<string>> AddParticipantsByModelsAsync(List<ParticipantModel> particitantModels, Guid eventId, Guid federationId)
        {
            var messageList = new List<string>();
            var existingTeamsBase = await GetExistingTeams(federationId);
            var eventCategories = await _categoryRepository.GetAll(w => w.EventId == eventId).ToDictionaryAsync(x => x.CategoryId, x => x.Name);
            var eventWeightDivisions = await _weightDivisionRepository.GetAll(w => w.Category.EventId == eventId).ToDictionaryAsync(x => x.WeightDivisionId, x => x.Name);
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
                var participant = new Participant
                {
                    ParticipantId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = DateTime.Parse(model.DateOfBirth),
                    EventId = eventId,
                    TeamId = ProcessTeam(model.Team, existingTeamsBase, teamsToAdd),
                    WeightDivisionId = eventWeightDivisions.First(x => x.Value.Equals(model.WeightDivision, StringComparison.OrdinalIgnoreCase)).Key,
                    CategoryId = eventCategories.First(x => x.Value.Equals(model.Category, StringComparison.OrdinalIgnoreCase)).Key,
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


        public List<ParticipantModel> GetOrderedListForBrackets(ParticitantFilterModel filter)
        {
            var fighters = GetParticitantModelsByFilter(filter);
            var count = fighters.Count;
            var bracketSize = GetBracketsSize(fighters.Count);
            for (var i = 0; i < bracketSize - count; i++)
            {
                fighters.Add(new ParticipantModel());
            }

            return Distribute(fighters);
        }
        
        #endregion
        
        #region Private methods

        private bool IsParticipantModelValid(ParticipantModel model, List<string> messageBuilder, int modelIndex, IDictionary<Guid,string> categories, IDictionary<Guid, string> weightDivisions)
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
            var weightDivision = weightDivisions.Values.FirstOrDefault(w => w.Equals(model.WeightDivision, StringComparison.OrdinalIgnoreCase));
            if (weightDivision == null)
            {
                messageBuilder.Add($"Weight division '{model.WeightDivision}' for '{identifier}' is invalid");
                isValid = false;
            }
            var category = categories.Values.FirstOrDefault(w => w.Equals(model.Category, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                messageBuilder.Add($"Category '{model.Category}' for '{identifier}' is invalid");
                isValid = false;
            }
            return isValid;
        }

        private List<ParticipantModel> Distribute(List<ParticipantModel> fightersList)
        {

            var orderedbyTeam = fightersList.ToList().GroupBy(f => f.Team).OrderByDescending(g => g.Count())
           .SelectMany(f => f).ToList();
            if (fightersList.Count > 2)
            {
                var sideA = new List<ParticipantModel>();
                var sideB = new List<ParticipantModel>();
                for (var i = 0; i < orderedbyTeam.Count; i++)
                {
                    var fighter = orderedbyTeam.ElementAtOrDefault(i);
                    if (i % 2 == 0)
                    {
                        sideA.Add(fighter);
                    }
                    else
                    {
                        sideB.Add(fighter);
                    }
                }
                return Distribute(sideA).Concat(Distribute(sideB)).ToList();
            }
            return fightersList;
        } 

        private List<ParticipantModel> GetModels(IEnumerable<Participant> fighters)
        {
            return fighters.Select(f => new ParticipantModel()
            {
                ParticipantId = f.ParticipantId,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Team = f.Team.Name,
                WeightDivision = f.WeightDivision.Name,
                DateOfBirth = f.DateOfBirth.ToString("yyyy-MM-dd")
            }).ToList();

        }

        private IQueryable<Participant> GetParticitants()
        {
            return _participantRepository.GetAll()
                .Include(f => f.Team).Include(f => f.WeightDivision);
        }

        private IQueryable<Participant> GetParticitantByFilter(ParticitantFilterModel filter)
        {
            return GetParticitants().Where(f => filter.CategoryIds.Contains(f.CategoryId)
                            && filter.WeightDivisionIds.Contains(f.WeightDivisionId));
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

        private DateTime? GetDateOfBirth(string stringDob)
        {
            //DateTime dob;
            if (DateTime.TryParse(stringDob, out var dob))
            {
                return dob;
            }
            return null;
        }

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
                        Name = name
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
        private WeightDivision GetWeightDivision(string name)
        {
            return _weightDivisionRepository.GetAll().FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        private Category GetCategory(string name)
        {
            return _categoryRepository.GetAll().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        }

        #endregion
        
        #region Brackets

        private const int FIGHTERS_MAX_COUNT = 64;

        private int GetBracketsSize(int fightersCount)
        {
            if (fightersCount == 3)
            {
                return 3;
            }
            for (var i = 1; i <= Math.Log(FIGHTERS_MAX_COUNT, 2); i++)
            {
                var size = Math.Pow(2, i);
                if (size >= fightersCount)
                {
                    return (int)size;
                }
            }
            return 2;
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
