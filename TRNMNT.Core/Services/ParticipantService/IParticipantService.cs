using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IParticipantService
    {
        Task CreateParticipant(Participant participant);
    }
}
