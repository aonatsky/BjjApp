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
        public FighterService(IRepository<Fighter> fighterRepository)
        {
            this.fighterRepository = fighterRepository;
        }

        public List<FighterModel> GetFighterModels()
        {
            //todo CHANGE
            return GetModels(fighterRepository.GetAll());
        }

        public List<FighterModel> GetFighterModelsByFilter(FighterFilterModel filter)
        {

            var fighters = GetFighters().Where(f => filter.Categories.Select(c => c.CategoryId).Contains(f.CategoryId)
                && filter.WeightDivisions.Select(wd => wd.WeightDivisionId).Contains(f.WeightDivisionId));
            return GetModels(fighters);
        }

        private List<FighterModel> GetModels(IEnumerable<Fighter> fighters)
        {
            return fighters.Select(f => new FighterModel()
            {
                FighterId = f.FighterId,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Team = f.Team.Name,
                Category = f.Category.Name,
                DateOfBirth = f.DateOfBirth.ToString("yyyy-mm-dd")
            }).ToList();

        }

        private IQueryable<Fighter> GetFighters()
        {
            return fighterRepository.GetAll().Include(f => f.Category)
                .Include(f => f.Team).Include(f => f.WeightDivision);
        }
    }
}
