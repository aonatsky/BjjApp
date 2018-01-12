using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services.Interface
{
    public interface IParticipantProcessingService
    {
        /// <summary>
        /// Gets the fighter models by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<ParticitantModel> GetParticitantModelsByFilter(ParticitantFilterModel filter);

        /// <summary>
        /// Gets the ordered list for brackets.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        List<ParticitantModel> GetOrderedListForBrackets(ParticitantFilterModel filter);

        /// <summary>
        /// Adds the fighters by models.
        /// </summary>
        /// <param name="particitantModels">The fighter models.</param>
        /// <param name="eventId">event participants uploaded for.</param>
        /// <returns></returns>
        Task<string> AddParticipantsByModelsAsync(List<ParticitantModel> particitantModels, Guid eventId);
    }
}
