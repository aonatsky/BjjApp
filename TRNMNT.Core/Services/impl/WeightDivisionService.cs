﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Enum;
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

        public async Task<List<WeightDivisionModelBase>> GetWeightDivisionModelsByCategoryIdAsync(Guid categoryId)
        {
            return (await GetWeightDivisionsByCategoryIdAsync(categoryId)).Select(wd =>
                new WeightDivisionModelBase
                {
                    WeightDivisionId = wd.WeightDivisionId,
                        Name = wd.Name
                }).ToList();
        }

        public async Task<List<WeightDivision>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId, bool includeCategory = false)
        {
            var query = _weightDevisionRepository.GetAll(wd => wd.CategoryId == categoryId);
            if (includeCategory)
            {
                query.Include(wd => wd.Category);
            }
            return await query.ToListAsync();
        }

        public async Task<List<WeightDivisionModel>> GetWeightDivisionModelsByEventIdAsync(Guid eventId, bool isWithAbsolute)
        {
            var query = _weightDevisionRepository.GetAll().Where(wd => wd.Category.EventId == eventId);
            if (!isWithAbsolute)
            {
                query = query.Where(w => !w.IsAbsolute);
            }
            var weightDivisions = await GetWeightDivisionsByEventIdAsync(eventId, isWithAbsolute);
            return weightDivisions.Select(wd =>
                GetWeightDivisionModel(wd)).ToList();
        }

        public async Task<List<WeightDivision>> GetWeightDivisionsByEventIdAsync(Guid eventId, bool isWithAbsolute)
        {
            var query = _weightDevisionRepository.GetAll().Where(wd => wd.Category.EventId == eventId);
            if (!isWithAbsolute)
            {
                query = query.Where(w => !w.IsAbsolute);
            }
            return await query.ToListAsync();
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

        public async Task<WeightDivision> GetWeightDivisionAsync(Guid weightDivisionId, bool includeCategory = false)
        {
            var query = _weightDevisionRepository.GetAll(wd => wd.WeightDivisionId == weightDivisionId);

            if (includeCategory)
            {
                query = query.Include(wd => wd.Category);
            }

            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region PrivateMethods
        private WeightDivisionModel GetWeightDivisionModel(WeightDivision wd)
        {
            var model = new WeightDivisionModel()
            {
                WeightDivisionId = wd.WeightDivisionId,
                Name = wd.Name,
                CategoryId = wd.CategoryId,
                Description = wd.Description,
                Weight = wd.Weight,

            };
            if (wd.StartTs != null)
            {
                model.Status = wd.CompleteTs != null ? (int) ProgressStatusEnum.InProgress : (int) ProgressStatusEnum.Completed;
            }
            else
            {
                model.Status = (int) ProgressStatusEnum.NotStarted;
            }

            return model;
        }

        public async Task SetWeightDivisionStartedAsync(Guid weightDivisionId)
        {
            var weightDivision = await _weightDevisionRepository.GetByIDAsync(weightDivisionId);
            weightDivision.StartTs = DateTime.UtcNow;
            _weightDevisionRepository.Update(weightDivision);
        }

        public async Task SetWeightDivisionCompletedAsync(Guid weightDivisionId)
        {
            var weightDivision = await _weightDevisionRepository.GetByIDAsync(weightDivisionId);
            weightDivision.CompleteTs = DateTime.UtcNow;
            _weightDevisionRepository.Update(weightDivision);
        }

        public async Task<bool> IsWeightDivisionCompletedAsync(Guid weightDivisionId)
        {
            return await _weightDevisionRepository.GetAll(wd => wd.WeightDivisionId == weightDivisionId).Select(wd => wd.CompleteTs != null).AnyAsync();
        }
        #endregion
    }
}