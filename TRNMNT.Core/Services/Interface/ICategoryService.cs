using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Category;

namespace TRNMNT.Core.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModelBase>> GetCategoriesByEventIdAsync(Guid eventId);
    }
}
