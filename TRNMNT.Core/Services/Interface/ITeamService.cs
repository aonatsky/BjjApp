using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Team;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface ITeamService : IPaidEntityService
    {
        /// <summary>
        /// Gets the team by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<Team> GetTeamByNameAsync(string name);

        /// <summary>
        /// Gets the teams asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TeamModel>> GetTeamsAsync();

        /// <summary>
        /// Gets teams where team members participate in Event with current id asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TeamModelBase>> GetTeamsAsync(Guid eventId);
        

        /// <summary>
        /// Processes the team registration asynchronous. Returns payment data
        /// </summary>
        /// <param name="federationId">The federation identifier.</param>
        /// <param name="model">The model.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="user">User</param>
        /// <returns></returns>
        Task<PaymentDataModel> ProcessTeamRegistrationAsync(Guid federationId, TeamModelFull model, string callbackUrl, string redirectUrl, User user);
        
    }
}
