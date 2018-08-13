using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Federation;

namespace TRNMNT.Core.Services.Interface
{
    public interface IFederationService
    {
        /// <summary>
        /// Returns price model for team registration;
        /// </summary>
        /// <param name="federationId">Federation Id</param>
        /// <returns></returns>
        Task<PriceModel> GetTeamRegistrationPriceAsync(Guid federationId);
        
        /// <summary>
        /// Returns price model for membership
        /// </summary>
        /// <param name="federationId"> Federation Id</param>
        /// <returns></returns>
        Task<PriceModel> GetMembershipPriceAsync(Guid federationId);

        /// <summary>
        /// Returns federation currency
        /// </summary>
        /// <param name="federationId"></param>
        /// <returns></returns>
        Task<string> GetFederationCurrencyAsync(Guid federationId);
        /// <summary>
        /// Returns federation model
        /// </summary>
        /// <param name="federationId">Federation Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns></returns>
        Task<FederationModel> GetFederationModelAsync(Guid federationId, string userId);

        /// <summary>
        /// Updates federation model
        /// </summary>
        /// <param name="model">Federation model.</param>
        /// /// <param name="userId">User Id.</param>
        /// <returns></returns>
        Task UpdateFederationAsync(FederationModel model, Guid federationId, string userId);
    }
}
