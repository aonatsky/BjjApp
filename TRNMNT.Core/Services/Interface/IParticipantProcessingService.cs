using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;

namespace TRNMNT.Core.Services.Interface
{
    public interface IParticipantProcessingService
    {
        /// <summary>
        /// Adds the fighters by models.
        /// </summary>
        /// <param name="particitantModels">The fighter models.</param>
        /// <param name="eventId">event participants uploaded for.</param>
        /// <param name="federationId">federation participants uploaded for.</param>
        /// <returns></returns>
        Task<List<string>> AddParticipantsByModelsAsync(List<ParticipantModel> particitantModels, Guid eventId, Guid federationId);
    }
}
