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
        Task<IEnumerable<WeightDivision>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<WeightDivisionModel>> GetWeightDivisionModelsByEventIdAsync(Guid eventId, bool isWithAbsolute);
        Task<WeightDivision> GetAbsoluteWeightDivisionAsync(Guid categoryId);
    }
}
