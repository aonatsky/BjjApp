using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model;

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
    }
}
