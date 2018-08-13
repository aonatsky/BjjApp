using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Federation;
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

        public async Task<FederationModel> GetFederationModelAsync(Guid federationId, string userId)
        {
            var federation = await _repository.GetByIDAsync(federationId);
            if (federation.OwnerId != userId)
            {
                return null;
            }
            return await _repository.GetAll().Where(f => f.FederationId == federationId).Select(f => new FederationModel()
            {
                Name = f.Name,
                    CommissionPercentage = f.CommissionPercentage,
                    ContactInformation = f.ContactInformation,
                    Description = f.Description,
                    ImgPath = f.ImgPath,
                    MembershipPrice = f.MembershipPrice,
                    MinCommission = f.MinCommission,
                    TeamRegistrationPrice = f.TeamRegistrationPrice
            }).FirstOrDefaultAsync();
        }

        public async Task UpdateFederationAsync(FederationModel model, Guid federationId, string userId)
        {
            var federation = await _repository.GetByIDAsync(federationId);
            if (federation.OwnerId == userId) { _repository.UpdateValues(federation, model); }
        }

        #endregion

    }
}