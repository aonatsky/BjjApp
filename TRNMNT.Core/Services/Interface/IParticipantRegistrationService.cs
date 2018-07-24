using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IParticipantRegistrationService
    {
        /// <summary>
        /// Processes the participant registration asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="model">The model.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <returns></returns>
        Task<ParticipantRegistrationResult> ProcessParticipantRegistrationAsync(Guid eventId, ParticipantRegistrationModel model, string callbackUrl, string redirectUrl, User user);
    }
}