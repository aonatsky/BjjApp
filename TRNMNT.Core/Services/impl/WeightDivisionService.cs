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
        #region Dependencies

        private readonly IRepository<WeightDivision> _weightDevisionRepository;

        #endregion

        #region .ctor

        public WeightDivisionService(IRepository<WeightDivision> weightDevisionRepository)
        {
            _weightDevisionRepository = weightDevisionRepository;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<WeightDivisionModelBase>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId)
        {
            return await _weightDevisionRepository.GetAll().Where(wd => wd.CategoryId == categoryId).Select(wd =>
                    new WeightDivisionModelBase
                    {
                        WeightDivisionId = wd.WeightDivisionId.ToString(),
                        Name = wd.Name
                    }).ToListAsync();
        }

        public async Task<IEnumerable<WeightDivisionModel>> GetWeightDivisionsByEventIdAsync(Guid eventId, bool isWithAbsolute)
        {
            var query = _weightDevisionRepository.GetAll().Where(wd => wd.Category.EventId == eventId);
            if (!isWithAbsolute)
            {
                query = query.Where(w => !w.IsAbsolute);
            }
            return await query.Select(wd =>
                new WeightDivisionModel
                {
                    WeightDivisionId = wd.WeightDivisionId.ToString(),
                    Name = wd.Name,
                    CategoryId = wd.CategoryId.ToString(),
                    Descritpion = wd.Descritpion,
                    Weight = wd.Weight
                }).ToListAsync();
        }

        public async Task<WeightDivision> GetAbsoluteWeightDivisionAsync(Guid categoryId)
        {
            var weightDivision = await _weightDevisionRepository.GetAll(w => w.CategoryId == categoryId && w.IsAbsolute).FirstOrDefaultAsync();
            if (weightDivision == null)
            {
                weightDivision = new WeightDivision
                {
                    WeightDivisionId = Guid.NewGuid(),
                    Name = "Absolute",
                    Weight = 0,
                    CategoryId = categoryId,
                    IsAbsolute = true,
                };
                _weightDevisionRepository.Add(weightDivision);
            }
            return weightDivision;
        }

        #endregion

    }
}
