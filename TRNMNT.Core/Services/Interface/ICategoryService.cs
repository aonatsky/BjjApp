using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Category;

namespace TRNMNT.Core.Services.Interface
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets the categories by event identifier asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<CategoryModelBase>> GetCategoriesByEventIdAsync(Guid eventId);
    }
}
