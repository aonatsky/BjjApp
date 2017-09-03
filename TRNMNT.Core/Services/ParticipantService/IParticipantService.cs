using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.Result;
using TRNMNT.Data.Entities;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public interface IParticipantService
    {

        Task<bool> IsParticipantExistsAsync(ParticipantModelBase participant);

        Task AddParticipant(Participant participant, bool saveContext = true);
    }
}
