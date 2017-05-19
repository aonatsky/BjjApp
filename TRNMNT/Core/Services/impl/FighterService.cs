using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.impl
{
    public class FighterService : IFighterService
    {
        private IRepository<Fighter> fighterRepository;
        private IRepository<Team> teamRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<WeightDivision> weightDivisionRepository;

        public FighterService(IRepository<Fighter> fighterRepository,
            IRepository<Team> teamRepository,
            IRepository<Category> categoryRepository,
            IRepository<WeightDivision> weightDivisionRepository)
        {
            this.fighterRepository = fighterRepository;
            this.teamRepository = teamRepository;
            this.categoryRepository = categoryRepository;
            this.weightDivisionRepository = weightDivisionRepository;
        }

        #region Public Methods

        public List<FighterModel> GetFighterModelsByFilter(FighterFilterModel filter)
        {
            IQueryable<Fighter> fighters = GetFightersByFilter(filter);
            return GetModels(fighters);
        }

        public string AddFightersByModels(List<FighterModel> fighterModels)
        {
            string message = "";
            var existingTeams = this.teamRepository.GetAll().ToList();
            var existingFighters = this.fighterRepository.GetAll().ToList();

            var fightersToAdd = new List<Fighter>();
            var teamsToAdd = new List<Team>();
            var comparer = new FighterComparer();
            foreach (var model in fighterModels)
            {
                var fighter = new Fighter()
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

                fighter.Team = ProcessTeam(model.Team, existingTeams, ref teamsToAdd);

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
                fightersToAdd.Add(fighter);
            }
            fighterRepository.AddRange(fightersToAdd);
            fighterRepository.Save();
            return message;
        }


        public List<FighterModel> GetOrderedListForBrackets(FighterFilterModel filter)
        {
            var fighters = GetFighterModelsByFilter(filter);
            var count = fighters.Count;
            var bracketSize = GetBracketsSize(fighters.Count());
            for (int i = 0; i < bracketSize - count; i++)
            {
                fighters.Add(new FighterModel() {});
            }

            return Distribute(fighters);
            //var orderedbyTeam = fighters.ToList().GroupBy(f => f.Team).OrderByDescending(g => g.Count())
            //.SelectMany(f => f).ToList();

            //List<FighterModel> sideA = new List<FighterModel>();
            //List<FighterModel> sideB = new List<FighterModel>();
            //for (int i = 0; i < bracketSize; i++)
            //{
            //    var fighter = orderedbyTeam.ElementAtOrDefault(i);
            //    if (i % 2 == 0)
            //    {
            //        sideA.Add(fighter);
            //    }
            //    else
            //    {
            //        sideB.Add(fighter);
            //    }
            //}
            //return GetDistributedSide(sideA).Concat(GetDistributedSide(sideB)).ToList();
            //return fighters;
            //return GetModels(fighters);
        }



        private List<FighterModel> GetDistributedSide(List<FighterModel> list)
        {
            var halfCount = list.Count() / 2;
            var result = new List<FighterModel>();
            for (int i = 0; i < halfCount; i++)
            {
                result.Add(list.ElementAtOrDefault(i));
                result.Add(list.ElementAtOrDefault(halfCount + i));
            }
            return result;
        }



        #endregion


        #region Private methods

        private List<FighterModel> Distribute(List<FighterModel> fightersList)
        {

            var orderedbyTeam = fightersList.ToList().GroupBy(f => f.Team).OrderByDescending(g => g.Count())
           .SelectMany(f => f).ToList();
            if (fightersList.Count() > 4)
            {
                List<FighterModel> sideA = new List<FighterModel>();
                List<FighterModel> sideB = new List<FighterModel>();
                for (int i = 0; i < orderedbyTeam.Count; i++)
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
                Category = f.Category.Name,
                DateOfBirth = f.DateOfBirth.ToString("yyyy-mm-dd")
            }).ToList();

        }

        private IQueryable<Fighter> GetFighters()
        {
            return fighterRepository.GetAll().Include(f => f.Category)
                .Include(f => f.Team).Include(f => f.WeightDivision);
        }

        private IQueryable<Fighter> GetFightersByFilter(FighterFilterModel filter)
        {
            return GetFighters().Where(f => filter.Categories.Select(c => c.CategoryId).Contains(f.CategoryId)
                            && filter.WeightDivisions.Select(wd => wd.WeightDivisionId).Contains(f.WeightDivisionId));
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
            else
            {
                return null;
            }
        }
        private Team ProcessTeam(string name, List<Team> teams, ref List<Team> teamsToAdd)
        {
            Team team = teamsToAdd.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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
            return weightDivisionRepository.GetAll().FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        private Category GetCategory(string name)
        {
            return (categoryRepository.GetAll().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

        }

        #endregion


        #region Brackets

        private const int FIGHTERS_MAX_COUNT = 64;

        private int GetBracketsSize(int fightersCount)
        {
            for (int i = 1; i <= Math.Log(FIGHTERS_MAX_COUNT, 2); i++)
            {
                var size = Math.Pow(2, i);
                if (size >= fightersCount)
                {
                    return (Int32)size;
                }
            }
            return 2;
        }

        #endregion

        private class FighterComparer : IEqualityComparer<Fighter>
        {
            public bool Equals(Fighter x, Fighter y)
            {
                return (x.FirstName == y.FirstName && x.LastName == y.LastName && x.DateOfBirth == y.DateOfBirth);
            }

            public int GetHashCode(Fighter fighter)
            {
                return fighter.FirstName.GetHashCode() ^ fighter.LastName.GetHashCode() ^ fighter.DateOfBirth.GetHashCode();
            }


        }


    }


}
