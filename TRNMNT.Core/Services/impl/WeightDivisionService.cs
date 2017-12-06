using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
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
            return (await weightdevisionRepository.GetAll().Where(wd => wd.CategoryId == categoryId).ToListAsync()).Select(wd => new WeightDivisionModelBase {WeightDivisionId = wd.WeightDivisionId.ToString(), Name = wd.Name });
        }
    }
}
