using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IWeightDivisionService
    {
        /// <summary>
        /// Gets the weight divisions by category identifier asynchronous.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        Task<List<WeightDivisionModelBase>> GetWeightDivisionModelsByCategoryIdAsync(Guid categoryId);
        Task<List<WeightDivision>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId, bool includeCategory = false);
        
        /// <summary>
        /// Gets weightdivision model by event Id asynchronous
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="isWithAbsolute"></param>
        /// <returns></returns>
        Task<List<WeightDivisionModel>> GetWeightDivisionModelsByEventIdAsync(Guid eventId, bool isWithAbsolute);
        
        /// <summary>
        /// Gets weightdivision by event Id asynchronous
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="isWithAbsolute"></param>
        /// <returns></returns>
        Task<List<WeightDivision>> GetWeightDivisionsByEventIdAsync(Guid eventId, bool isWithAbsolute);
        
        Task<WeightDivision> GetAbsoluteWeightDivisionAsync(Guid categoryId);
        /// <summary>
        /// Gets weightdivision by Id asynchronous.
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <param name="includeCategory"></param>
        /// <returns></returns>
        Task<WeightDivision> GetWeightDivisionAsync(Guid weightDivisionId, bool includeCategory = false);

        Task SetWeightDivisionStartedAsync(Guid weightDivisionId);
        Task SetWeightDivisionCompletedAsync(Guid weightDivisionId);
        /// <summary>
        /// Checks if weightdivision completed
        /// </summary>
        /// <param name="weightDivisionId">Id</param>
        /// <returns></returns>
        Task<bool> IsWeightDivisionCompletedAsync(Guid weightDivisionId);
    }
}
