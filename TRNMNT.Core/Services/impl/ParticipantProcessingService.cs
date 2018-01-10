using System;
using System.Collections.Generic;
using System.Linq;
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

        public string AddParticipantsByModels(List<ParticitantModel> fighterModels)
        {
            var message = string.Empty;
            var existingTeamsNames = _teamRepository.GetAll().Select(x => x.Name).ToList();
            var existingParticipants = _participantRepository.GetAll().ToList();

            var participantsToAdd = new List<Participant>();
            var teamsToAdd = new List<Team>();
            var comparer = new ParticipantComparer();
            foreach (var model in fighterModels)
            {
                var participant = new Participant
                {
                    ParticipantId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                if (DateTime.TryParse(model.DateOfBirth, out var dob))
                {
                    participant.DateOfBirth = dob;
                }
                else
                {
                    message += $"Date of birth for {participant.FirstName} {participant.LastName} is invalid";
                    continue;
                }

                participant.Team = ProcessTeam(model.Team, existingTeamsNames, teamsToAdd);

                var weightDivision = GetWeightDivision(model.WeightDivision);
                if (weightDivision != null)
                {
                    participant.WeightDivisionId = weightDivision.WeightDivisionId;
                }
                else
                {
                    message += $"Weight division {model.WeightDivision} is invalid";
                    continue;
                }

                var category = GetCategory(model.Category);
                if (category != null)
                {
                    participant.CategoryId = category.CategoryId;
                }
                else
                {
                    message += $"Category {model.Category} is invalid ";
                    continue;
                }
                if (!existingParticipants.Contains(participant, comparer) && !participantsToAdd.Contains(participant, comparer))
                {
                    participantsToAdd.Add(participant);
                }
            }
            _participantRepository.AddRange(participantsToAdd);
            return message;
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
