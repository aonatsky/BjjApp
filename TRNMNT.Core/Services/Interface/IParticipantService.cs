using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Interface;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IParticipantService : IPaidEntityService
    {
    
        /// <summary>
        /// Determines whether [is participant exists asynchronous].
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
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
        /// <param name="eventId">evetId participants selected for</param>
        /// <returns>filtered ParticipantTableModel list</returns>
        Task<IPagedList<ParticipantTableModel>> GetFilteredParticipantsAsync(ParticipantFilterModel eventId);

        /// <summary>
        /// Deletes paraticipant by id
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        Task DeleteParticipantAsync(Guid participantId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantModel"></param>
        /// <returns></returns>
        Task<Participant> UpdateParticipantAsync(ParticipantTableModel participantModel);

        /// <summary>
        /// Returns participants for particular weightdivision
        /// </summary>
        /// <param name="weightDivisionId"></param>
        /// <param name="includeTeam"></param>
        /// <param name="showInactive"></param>
        /// <returns>List of participants</returns>
        Task<IEnumerable<Participant>> GetParticipantsByWeightDivisionAsync(Guid weightDivisionId, bool includeTeam = false, bool showInactive = false);

        /// <summary>
        /// Returns participants for absolute division by category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>List of participants</returns>
        Task<IEnumerable<Participant>> GetParticipantsInAbsoluteDivisionByCategoryAsync(Guid categoryId);

        Task AddParticipantsToAbsoluteWeightDivisionAsync(Guid[] participantsIds, Guid categoryId, Guid absoluteWeightDivisionId);

    }
}
