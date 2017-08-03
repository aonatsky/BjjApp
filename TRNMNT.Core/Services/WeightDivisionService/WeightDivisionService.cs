using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using System.Linq;

namespace TRNMNT.Core.Services
{
    public class WeightDivisionService : IWeightDivisionService
    {
        private IRepository<WeightDivision> weightdevisionRepository;

        public WeightDivisionService(IRepository<WeightDivision> weightdevisionRepository)
        {
            this.weightdevisionRepository = weightdevisionRepository;
        }

        public async Task<List<WeightDivision>> GetWeightDivisionsByCategoryId(Guid categoryId)
        {
            return await weightdevisionRepository.GetAll().Where(wd => wd.CategoryId == categoryId).ToListAsync();
        }
    }
}
