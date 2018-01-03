using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Services.Interface
{
    public interface IWeightDivisionService
    {
        /// <summary>
        /// Gets the weight divisions by category identifier asynchronous.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<WeightDivisionModelBase>> GetWeightDivisionsByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<WeightDivisionModel>> GetWeightDivisionsByEventIdAsync(Guid eventId);
    }
}
