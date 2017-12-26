using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using System.Linq;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Services
{
    public class WeightDivisionService : IWeightDivisionService
    {
        private IRepository<WeightDivision> weightdevisionRepository;

        public WeightDivisionService(IRepository<WeightDivision> weightdevisionRepository)
        {
            this.weightdevisionRepository = weightdevisionRepository;
        }

        public async Task<IEnumerable<WeightDivisionModelBase>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId)
        {
            return await weightdevisionRepository.GetAll().Where(wd => wd.CategoryId == categoryId).Select(wd =>
                    new WeightDivisionModelBase
                    {
                        WeightDivisionId = wd.WeightDivisionId.ToString(),
                        Name = wd.Name
                    }).ToListAsync();
        }
    }
}
