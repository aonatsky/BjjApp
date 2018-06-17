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
        Task<IEnumerable<WeightDivisionModelBase>> GetWeightDivisionModelsByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<WeightDivision>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId, bool includeCategory = false);
        Task<IEnumerable<WeightDivisionModel>> GetWeightDivisionModelsByEventIdAsync(Guid eventId, bool isWithAbsolute);
        Task<WeightDivision> GetAbsoluteWeightDivisionAsync(Guid categoryId);
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
