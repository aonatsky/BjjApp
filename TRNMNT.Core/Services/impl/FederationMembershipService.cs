using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class FederationMembershipService : IFederationMembershipService
    {
        #region Dependencies

        private readonly IRepository<FederationMembership> _repository;

        #endregion

        #region .ctor

        public FederationMembershipService(IRepository<FederationMembership> federationMembershipRepository)
        {
            _repository = federationMembershipRepository;
        }

        #endregion

        #region public methods
        public async Task<bool> IsFederationMemberAsync(Guid federationId, string userId)
        {
            return await _repository.GetAll().AnyAsync(fm => fm.UserId == userId && fm.FederationId == federationId);
        }

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var membership = await _repository.GetByIDAsync(entityId);
            membership.UpdateTs = DateTime.UtcNow;
            membership.IsApproved = true;
            _repository.Update(membership);
        }
        #endregion

    }
}