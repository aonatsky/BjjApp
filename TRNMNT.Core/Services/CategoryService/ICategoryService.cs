using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesByEventId(Guid eventId);
    }
}
