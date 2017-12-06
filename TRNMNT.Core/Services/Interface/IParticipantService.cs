using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IParticipantService : IPaidEntityService
    {

        Task<bool> IsParticipantExistsAsync(ParticipantModelBase participant, Guid eventId);

        /// <summary>
        /// Adds participant to specified event. 
        /// </summary>
        /// <param name="participant">Participant to add</param>
        /// <returns>Participant Id</returns>
        void AddParticipant(Participant participant);

        /// <summary>
        /// Creates new participant to save from model
        /// </summary>
        /// <param name="model">Registration Model</param>
        /// <param name="eventId">Event Id</param>
        /// <returns></returns>
        Participant CreatePaticipant(ParticipantRegistrationModel model, Guid eventId);
    }
}
