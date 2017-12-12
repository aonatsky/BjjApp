using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IParticipantService : IPaidEntityService
    {

        Task<bool> IsParticipantExistsAsync(ParticipantModelBase participant, Guid eventId);

        /// <summary>
        /// Adds participant to specified event. 
        /// </summary>
        /// <param name="participant">Participant to add</param>
        /// <returns></returns>
        void AddParticipant(Participant participant);

        /// <summary>
        /// Creates new participant to save from model
        /// </summary>
        /// <param name="model">Registration Model</param>
        /// <param name="eventId">Event Id</param>
        /// <returns></returns>
        Participant CreatePaticipant(ParticipantRegistrationModel model, Guid eventId);

        /// <summary>
        /// Select ParticipantTableModel list for specified event with filtration.
        /// </summary>
        /// <param name="federationId">dependent federation id</param>
        /// <param name="eventId">evetId participants selected for</param>
        /// <returns>filtered ParticipantTableModel list</returns>
        Task<List<ParticipantTableModel>> GetFilteredParticipantsAsync(Guid federationId, Guid eventId);
    }
}
