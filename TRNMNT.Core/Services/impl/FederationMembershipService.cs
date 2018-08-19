using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class FederationMembershipService : IFederationMembershipService
    {
        #region Dependencies

        private readonly IRepository<FederationMembership> _repository;
        private readonly IFederationService _federationService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        #endregion

        #region .ctor

        public FederationMembershipService(IRepository<FederationMembership> federationMembershipRepository,
            IFederationService federationService, IOrderService orderService,
            IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _repository = federationMembershipRepository;
            _federationService = federationService;
        }

        #endregion

        #region public methods
        public async Task<bool> IsFederationMemberAsync(Guid federationId, string userId)
        {
            var membership = await _repository.GetAll().FirstOrDefaultAsync(fm => fm.UserId == userId && fm.FederationId == federationId);
            if (membership == null || membership.ApprovalStatus == ApprovalStatus.Declined)
            {
                return false;
            }
            await CheckMembershipStatusAsync(membership);
            return membership.ApprovalStatus == ApprovalStatus.Approved;
        }

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var membership = await _repository.GetByIDAsync(entityId);
            membership.UpdateTs = DateTime.UtcNow;
            membership.ApprovalStatus = ApprovalStatus.Approved;
            _repository.Update(membership);
        }

        public async Task<PaymentDataModel> ProcessFederationMembershipAsync(Guid federationId, string callbackUrl, string redirectUrl, User user)
        {
            if (await IsFederationMemberAsync(federationId, user.Id))
            {
                throw new BusinessException("PARTICIPATION.PARTICIPANT_IS_ALREADY_MEMBER");
            }
            var federationMembershipId = Guid.NewGuid();
            var priceModel = await _federationService.GetTeamRegistrationPriceAsync(federationId);
            var order = _orderService.AddNewOrder(OrderTypeEnum.FederationMembership, priceModel.Amount, priceModel.Currency, federationId.ToString(), user.Id);
            AddFederationMembership(user.Id, federationId, order.OrderId, federationMembershipId);
            return _paymentService.GetPaymentDataModel(order, callbackUrl, redirectUrl);
        }

        #endregion

        #region
        public void AddFederationMembership(string userId, Guid federationId, Guid orderId, Guid federationMembershipId)
        {
            var federationMembership = new FederationMembership()
            {
                FederationMembershipId = federationMembershipId,
                ApprovalStatus = ApprovalStatus.Pending,
                OrderId = orderId,
                UserId = userId,
                CreateTs = DateTime.UtcNow,
                FederationId = federationId
            };
            _repository.Add(federationMembership);
        }
        #endregion

        private async Task CheckMembershipStatusAsync(FederationMembership membership)
        {
            if (membership.ApprovalStatus == ApprovalStatus.Pending && membership.OrderId.HasValue)
            {
                membership.ApprovalStatus = await _orderService.GetApprovalStatus(membership.OrderId.Value);
                _repository.Update(membership);
            }
        }

        public async Task<List<FederationMembership>> GetFederationMembershipsForUsersAsync(Guid federationId, List<string> userIds)
        {
            return await _repository.GetAll(fm => userIds.Contains(fm.UserId)).ToListAsync();
        }
    }
}