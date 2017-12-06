using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IEmailService
    {
        Task SendForgotPasswordEmail(User user);
        Task SendEventParticipationEmail(User user, Participant participant, Event _event);
        Task SendEventParticipationApprovalEmail(User user, Event _event);
        Task SendTeamRegistrationEmail(User user, Team team);
        Task SendMultipleParticipationEmail(User user, IEnumerable<Participant> participants, Team team);
    }
}
