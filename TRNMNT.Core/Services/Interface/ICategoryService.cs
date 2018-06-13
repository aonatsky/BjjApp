using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Category;
using TRNMNT.Data.Entities;

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

        /// <summary>
        /// Returns completed categories by event identifier asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier</param>
        /// <returns></returns>
        Task<IEnumerable<CategoryModelBase>> GetCompletedCategoriesByEventIdAsync(Guid eventId);

        /// <summary>
        /// Returns category by category identifier async. 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<Category> GetCategoryAsync(Guid categoryId);

        /// <summary>
        /// Checks if category completed. 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<bool> IsCategoryCompletedAsync(Guid categoryId);
    }
}
