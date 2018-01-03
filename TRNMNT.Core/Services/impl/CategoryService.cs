using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                CategoryId = c.CategoryId.ToString(),
                Name = c.Name
            }).ToListAsync();
        }

        #endregion
    }
}
