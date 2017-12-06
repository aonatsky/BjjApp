﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Category;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private IRepository<Category> categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryModelBase>> GetCategoriesByEventIdAsync(Guid eventId)
        {
            return (await categoryRepository.GetAll().Where(c => c.EventId == eventId).ToListAsync()).Select(c => new CategoryModelBase { CategoryId = c.CategoryId.ToString(), Name = c.Name });
        }

    }
}
