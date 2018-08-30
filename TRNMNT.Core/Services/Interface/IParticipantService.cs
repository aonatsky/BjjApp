using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
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
        /// Determines whether [is participant exists asynchronous].
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<bool> IsParticipantExistsAsync(string userId, Guid eventId);

        /// Adds new participant;
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eventId"></param>
        /// <param name="orderId"></param>
        /// <param name="participantId"></param>
        void AddParticipant(ParticipantRegistrationModel model, Guid eventId, Guid orderId, Guid participantId, bool isFederationMember);

        /// <summary>
        /// Select ParticipantTableModel list for specified event with filtration.
        /// </summary>
        /// <param name="eventId">eventId participants selected for</param>
        /// <returns>filtered ParticipantTableModel list</returns>
        Task<IPagedList<ParticipantTableModel>> GetFilteredParticipantsAsync(ParticipantFilterModel eventId);

        /// <summary>
        /// Returns participants for user.
        /// </summary>
        /// <param name="userId"> User Id.</param>
        /// <param name="isTeamOwner">Is team owner</param>
        /// <returns></returns>
        Task<List<ParticipantEventModel>> GetUserParticipantsAsync(User user, bool isTeamOwner);

        /// <summary>
        /// Deletes participant by id
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

        /// <summary>
        /// Processes the participant registration asynchronous. Returns payment data
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="model">The model.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="user">User</param>
        /// <returns></returns>
        Task<PaymentDataModel> ProcessParticipantRegistrationAsync(Guid eventId, Guid federationId, ParticipantRegistrationModel model, string callbackUrl, string redirectUrl, User user);

        /// <summary>
        /// Processes the participant registration asynchronous. Returns payment data
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="model">The model.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="teamOwner">Team Owner user</param>
        /// <returns></returns>
        Task<PaymentDataModel> ProcessParticipantTeamRegistrationAsync(Guid eventId, Guid federationId, List<ParticipantRegistrationModel> models, string callbackUrl, string redirectUrl, User teamOwner);

        /// <summary>
        /// Set participant weight in status
        /// </summary>
        /// <param name="participantId"> participant Id</param>
        /// <param name="status">status, can be pending, declined or approved</param>
        /// <returns></returns>
        Task SetWeightInStatus(Guid participantId, string status);
    }
}