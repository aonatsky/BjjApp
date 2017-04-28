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
            return GetModels(fighterRepository.GetAll());
        }

        public List<FighterModel> GetFighterModelsByFilter(FighterFilterModel filter)
        {
            
            var fighters = fighterRepository.GetAll().Where(f => filter.Categories.Contains(f.Category) && filter.WeightDivisions.Contains(f.WeightDivision));
            return GetModels(fighters);
        }

        private List<FighterModel> GetModels(IEnumerable<Fighter> fighters){
            return fighters.Select(f => new FighterModel(){
                FighterId = f.FighterId,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Team = f.Team.Name,
                Category = f.Category.Name,
                DateOfBirth = f.DateOfBirth.ToString("yyyy-mm-dd")
            }).ToList();

        }
    }
}
