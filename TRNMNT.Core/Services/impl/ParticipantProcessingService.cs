using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
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

        public List<ParticitantModel> GetParticitantModelsByFilter(ParticitantFilterModel filter)
        {
            var fighters = GetParticitantByFilter(filter);
            return GetModels(fighters);
        }

        public async Task<string> AddParticipantsByModelsAsync(List<ParticitantModel> particitantModels, Guid eventId)
        {
            var messageBuilder = new StringBuilder();
            var existingTeamsNames = await _teamRepository.GetAll().Select(x => x.Name).ToListAsync();
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
                if (!IsParticipantModelValid(model, messageBuilder, index, eventCategories, eventWeightDivisions))
                {
                    continue;
                }
                var participant = new Participant
                {
                    ParticipantId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = DateTime.Parse(model.DateOfBirth),
                    Team = ProcessTeam(model.Team, existingTeamsNames, teamsToAdd),
                    WeightDivisionId = eventWeightDivisions.First(x => x.Value.Equals(model.WeightDivision, StringComparison.OrdinalIgnoreCase)).Key,
                    CategoryId = eventCategories.First(x => x.Value.Equals(model.Category, StringComparison.OrdinalIgnoreCase)).Key,
                };

                if (!existingParticipants.Contains(participant, comparer) && !participantsToAdd.Contains(participant, comparer))
                {
                    participantsToAdd.Add(participant);
                }
            }
            _participantRepository.AddRange(participantsToAdd);
            return messageBuilder.ToString();
        }


        public List<ParticitantModel> GetOrderedListForBrackets(ParticitantFilterModel filter)
        {
            var fighters = GetParticitantModelsByFilter(filter);
            var count = fighters.Count;
            var bracketSize = GetBracketsSize(fighters.Count);
            for (var i = 0; i < bracketSize - count; i++)
            {
                fighters.Add(new ParticitantModel());
            }

            return Distribute(fighters);
        }
        
        #endregion
        
        #region Private methods

        private bool IsParticipantModelValid(ParticitantModel model, StringBuilder messageBuilder, int modelIndex, IDictionary<Guid,string> categories, IDictionary<Guid, string> weightDivisions)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(model.FirstName))
            {
                messageBuilder.AppendLine($"First Name for Particitant with index {modelIndex} is invalid");
                isValid = false;
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                messageBuilder.AppendLine($"Last Name for Particitant with index {modelIndex}  is invalid");
                isValid = false;
            }
            var nValid = !string.IsNullOrEmpty(model.FirstName) && !string.IsNullOrEmpty(model.LastName);
            var identifier = nValid ? $"{model.FirstName} {model.LastName}" : $"Particitant with index {modelIndex}";

            if (!DateTime.TryParse(model.DateOfBirth, out _))
            {
                messageBuilder.AppendLine($"Date of birth for '{identifier}' is invalid");
                isValid = false;
            }
            if (string.IsNullOrEmpty(model.Team))
            {
                messageBuilder.AppendLine($"Team name for '{identifier}'  is invalid");
                isValid = false;
            }
            var weightDivision = weightDivisions.Values.FirstOrDefault(w => w.Equals(model.WeightDivision, StringComparison.OrdinalIgnoreCase));
            if (weightDivision == null)
            {
                messageBuilder.AppendLine($"Weight division '{model.WeightDivision}' for '{identifier}' is invalid");
                isValid = false;
            }
            var category = categories.Values.FirstOrDefault(w => w.Equals(model.Category, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                messageBuilder.AppendLine($"Category '{model.Category}' for '{identifier}' is invalid");
                isValid = false;
            }
            return isValid;
        }

        private List<ParticitantModel> Distribute(List<ParticitantModel> fightersList)
        {

            var orderedbyTeam = fightersList.ToList().GroupBy(f => f.Team).OrderByDescending(g => g.Count())
           .SelectMany(f => f).ToList();
            if (fightersList.Count > 2)
            {
                var sideA = new List<ParticitantModel>();
                var sideB = new List<ParticitantModel>();
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

        private List<ParticitantModel> GetModels(IEnumerable<Participant> fighters)
        {
            return fighters.Select(f => new ParticitantModel()
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

        private Team ProcessTeam(string name, IEnumerable<string> existingTeamNames, List<Team> teamsToAdd)
        {
            var team = teamsToAdd.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (team == null)
            {
                var teamName = existingTeamNames.FirstOrDefault(tn => tn.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (teamName == null)
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
                    return _teamRepository.GetAll(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).First();
                }
            }
            return team;

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
