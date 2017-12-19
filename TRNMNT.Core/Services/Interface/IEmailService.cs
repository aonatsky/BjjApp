using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends the forgot password email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task SendForgotPasswordEmail(User user);

        /// <summary>
        /// Sends the event participation email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="participant">The participant.</param>
        /// <param name="_event">The event.</param>
        /// <returns></returns>
        Task SendEventParticipationEmail(User user, Participant participant, Event _event);

        /// <summary>
        /// Sends the event participation approval email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="_event">The event.</param>
        /// <returns></returns>
        Task SendEventParticipationApprovalEmail(User user, Event _event);

        /// <summary>
        /// Sends the team registration email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="team">The team.</param>
        /// <returns></returns>
        Task SendTeamRegistrationEmail(User user, Team team);

        /// <summary>
        /// Sends the multiple participation email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="participants">The participants.</param>
        /// <param name="team">The team.</param>
        /// <returns></returns>
        Task SendMultipleParticipationEmail(User user, IEnumerable<Participant> participants, Team team);
    }
}
