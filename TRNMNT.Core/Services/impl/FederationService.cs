using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class FederationService : IFederationService
    {
        #region Dependencies

        private readonly IRepository<Federation> _repository;

        #endregion

        #region .ctor

        public FederationService(IRepository<Federation> federationRepository)
        {
            _repository = federationRepository;
        }

        #endregion

        #region public methods

        public async Task<PriceModel> GetTeamRegistrationPriceAsync(Guid federationId)
        {
            return await _repository.GetAll(f => f.FederationId == federationId).
            Select(f => new PriceModel()
            {
                Amount = f.TeamRegistrationPrice, Currency = f.Currency
            }).FirstOrDefaultAsync();
        }

        public async Task<PriceModel> GetMembershipPriceAsync(Guid federationId)
        {
            return await _repository.GetAll(f => f.FederationId == federationId).
            Select(f => new PriceModel()
            {
                Amount = f.MembershipPrice, Currency = f.Currency
            }).FirstOrDefaultAsync();
        }

        public async Task<string> GetFederationCurrencyAsync(Guid federationId)
        {
            return await _repository.GetAll(f => f.FederationId == federationId).Select(f => f.Currency).FirstOrDefaultAsync();
        }

        #endregion

    }
}