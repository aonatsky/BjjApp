using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using System.Linq;

namespace TRNMNT.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private IRepository<Category> categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetCategoriesByEventId(Guid eventId)
        {
            return await categoryRepository.GetAll().Where(c => c.EventId == eventId).ToListAsync();
        }
    }
}
