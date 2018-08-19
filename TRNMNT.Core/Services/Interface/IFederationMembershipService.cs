using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFederationMembershipService : IPaidEntityService
    {
        /// <summary>
        /// Checks if user is a member of selected federation.
        /// </summary>
        /// <param name="federationId">Federation Id</param>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task<bool> IsFederationMemberAsync(Guid federationId, string userId);

        /// <summary>
        /// Gets federation memberships for users
        /// </summary>
        /// <param name="federationId"> Federation Id.</param>
        /// <param name="userIds">User's Ids.</param>
        /// <returns></returns>
        Task<List<FederationMembership>> GetFederationMembershipsForUsersAsync(Guid federationId, List<string> userIds);

        /// <summary>
        /// Returns payment data for membersip registration.
        /// </summary>
        /// <param name="federationId"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<PaymentDataModel> ProcessFederationMembershipAsync(Guid federationId, string callbackUrl, string redirectUrl, User user);

        /// <summary>
        /// Adds federation membership
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="federationId"></param>
        /// <param name="orderId"></param>
        /// <param name="federationMembershipId"></param>
        void AddFederationMembership(string userId, Guid federationId, Guid orderId, Guid federationMembershipId);
    }
}