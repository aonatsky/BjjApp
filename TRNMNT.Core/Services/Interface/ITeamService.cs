using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Team;
using TRNMNT.Core.Model.User;
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
        /// Returns teams registered in the federation for event participation;
        /// </summary>
        /// <param name="federationId"></param>
        /// <returns></returns>
        Task<IEnumerable<TeamModelBase>> GetTeamsForEventAsync(Guid federationId);

        /// <summary>
        /// Returns teams registered in the federation for admin overview;
        /// </summary>
        /// <param name="federationId"></param>
        /// <returns></returns>
        Task<IEnumerable<TeamModelFull>> GetTeamsForAdminAsync(Guid federationId);

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

        /// <summary>
        ///  Returns athletes for the team;
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <returns></returns>
        Task<IEnumerable<UserModelAthlete>> GetAthletes(Guid teamId);

        /// <summary>
        ///  Returns athletes for the team;
        /// </summary>
        /// <param name="user">User object.</param>
        /// <returns></returns>
        Task<UserModelAthlete> GetAthlete(User user);

        /// <summary>
        /// Retruns team for owner
        /// </summary>
        /// <param name="ownerId"> Owner Id.</param>
        /// <returns></returns>
        Task<Team> GetTeamForOwnerAsync(string ownerId);
    }
}