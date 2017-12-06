using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class FighterService : IFighterService
    {
        #region Dependencies

        private readonly IRepository<Fighter> _fighterRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly IRepository<WeightDivision> _categoryRepository;
        private readonly IRepository<WeightDivision> _weightDivisionRepository;
        private readonly IAppDbContext _unitOfWork;


        #endregion

        #region .ctor

        public FighterService(IRepository<Fighter> fighterRepository,
            IRepository<Team> teamRepository,
            IRepository<WeightDivision> categoryRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IAppDbContext unitOfWork)

        {
            _fighterRepository = fighterRepository;
            _teamRepository = teamRepository;
            _categoryRepository = categoryRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion
        
        #region Public Methods

        public List<FighterModel> GetFighterModelsByFilter(FighterFilterModel filter)
        {
            var fighters = GetFightersByFilter(filter);
            return GetModels(fighters);
        }

        public string AddFightersByModels(List<FighterModel> fighterModels)
        {
            var message = string.Empty;
            var existingTeams = _teamRepository.GetAll().ToList();
            var existingFighters = _fighterRepository.GetAll().ToList();

            var fightersToAdd = new List<Fighter>();
            var teamsToAdd = new List<Team>();
            var fighterComparer = new FighterComparer();
            foreach (var model in fighterModels)
            {
                var fighter = new Fighter
                {
                    FighterId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                if (DateTime.TryParse(model.DateOfBirth, out var dob))
                {
                    fighter.DateOfBirth = dob;
                }
                else
                {
                    message += $"Date of birth for {fighter.FirstName} {fighter.LastName} is invalid";
                    continue;
                }

                fighter.Team = ProcessTeam(model.Team, existingTeams, teamsToAdd);

                var weightDivision = GetWeightDivision(model.WeightDivision);
                if (weightDivision != null)
                {
                    fighter.WeightDivisionId = weightDivision.WeightDivisionId;
                }
                else
                {
                    message += $"Weight division {model.WeightDivision} is invalid";
                    continue;
                }

                var category = GetCategory(model.Category);
                if (category != null)
                {
                    fighter.CategoryId = category.CategoryId;
                }
                else
                {
                    message += $"Category {model.Category} is invalid ";
                    continue;
                }
                if (!existingFighters.Contains(fighter,fighterComparer) && !fightersToAdd.Contains(fighter,fighterComparer))
                {
                    fightersToAdd.Add(fighter);
                }
            }
            _fighterRepository.AddRange(fightersToAdd);
            _unitOfWork.Save();
            return message;
        }


        public List<FighterModel> GetOrderedListForBrackets(FighterFilterModel filter)
        {
            var fighters = GetFighterModelsByFilter(filter);
            var count = fighters.Count;
            var bracketSize = GetBracketsSize(fighters.Count);
            for (var i = 0; i < bracketSize - count; i++)
            {
                fighters.Add(new FighterModel());
            }

            return Distribute(fighters);
        }
        
        #endregion
        
        #region Private methods

        private List<FighterModel> Distribute(List<FighterModel> fightersList)
        {

            var orderedbyTeam = fightersList.ToList().GroupBy(f => f.Team).OrderByDescending(g => g.Count())
           .SelectMany(f => f).ToList();
            if (fightersList.Count > 2)
            {
                var sideA = new List<FighterModel>();
                var sideB = new List<FighterModel>();
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

        private List<FighterModel> GetModels(IEnumerable<Fighter> fighters)
        {
            return fighters.Select(f => new FighterModel()
            {
                FighterId = f.FighterId,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Team = f.Team.Name,
                WeightDivision = f.WeightDivision.Name,
                DateOfBirth = f.DateOfBirth.ToString("yyyy-MM-dd")
            }).ToList();

        }

        private IQueryable<Fighter> GetFighters()
        {
            return _fighterRepository.GetAll()
                .Include(f => f.Team).Include(f => f.WeightDivision);
        }

        private IQueryable<Fighter> GetFightersByFilter(FighterFilterModel filter)
        {
            return GetFighters().Where(f => filter.CategoryIds.Contains(f.CategoryId)
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

        private Team ProcessTeam(string name, List<Team> teams, List<Team> teamsToAdd)
        {
            var team = teamsToAdd.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (team == null)
            {
                team = teams.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (team == null)
                {
                    team = new Team
                    {
                        TeamId = Guid.NewGuid(),
                        Name = name
                    };
                    teamsToAdd.Add(team);
                }
            }
            return team;

        }
        private WeightDivision GetWeightDivision(string name)
        {
            return _weightDivisionRepository.GetAll().FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        private WeightDivision GetCategory(string name)
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

        private class FighterComparer : IEqualityComparer<Fighter>
        {
            public bool Equals(Fighter x, Fighter y)
            {
                return x.FirstName == y.FirstName && x.LastName == y.LastName && x.DateOfBirth == y.DateOfBirth;
            }

            public int GetHashCode(Fighter fighter)
            {
                return fighter.FirstName.GetHashCode() ^ fighter.LastName.GetHashCode() ^ fighter.DateOfBirth.GetHashCode();
            }
        }
    }
}
