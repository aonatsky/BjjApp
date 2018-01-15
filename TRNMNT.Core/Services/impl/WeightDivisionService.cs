﻿using System;
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

        public async Task<IEnumerable<WeightDivisionModel>> GetWeightDivisionsByEventIdAsync(Guid eventId)
        {
            return await _weightDevisionRepository.GetAll().Where(wd => wd.Category.EventId == eventId).Select(wd =>
                    new WeightDivisionModel
                    {
                        WeightDivisionId = wd.WeightDivisionId.ToString(),
                        Name = wd.Name,
                        CategoryId = wd.CategoryId.ToString(),
                        Descritpion = wd.Descritpion,
                        Weight = wd.Weight
                    }).ToListAsync();
        }

        #endregion

    }
}
