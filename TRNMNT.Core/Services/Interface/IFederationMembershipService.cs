using System;
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
        /// <param name="federationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsFederationMemberAsync(Guid federationId, string userId);

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