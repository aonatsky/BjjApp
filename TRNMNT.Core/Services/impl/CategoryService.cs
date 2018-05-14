﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        #region Dependencies

        private readonly IRepository<Category> _categoryRepository;

        #endregion

        #region .ctor

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<CategoryModelBase>> GetCategoriesByEventIdAsync(Guid eventId)
        {
            return await _categoryRepository.GetAll().Where(c => c.EventId == eventId).Select(c => new CategoryModelBase
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToListAsync();
        }

        public async Task SetCategoryCompleteAsync(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIDAsync(categoryId);
            if (category != null)
            {
                category.CompleteTs = DateTime.UtcNow;
            }
            _categoryRepository.Update(category);
        }

        public async Task<Category> GetCategoryAsync(Guid categoryId)
        {
            return await _categoryRepository.GetByIDAsync(categoryId);
        }

        public async Task<IEnumerable<CategoryModelBase>> GetCompletedCategoriesByEventIdAsync(Guid eventId)
        {
            return await _categoryRepository.GetAll().Where(c => c.EventId == eventId && c.CompleteTs != null).Select(c => new CategoryModelBase
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToListAsync();
        }

        #endregion


        #region Private Methods



        #endregion

    }
}
