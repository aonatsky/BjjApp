using System;
using System.Threading.Tasks;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class OrderService : IOrderService
    {
        #region Dependencies

        private readonly IRepository<Order> _repository;
        private readonly IPaymentService _paymentService;

        #endregion

        #region .ctor

        public OrderService(IRepository<Order> orderRepository, IPaymentService paymentService)
        {
            _repository = orderRepository;
            _paymentService = paymentService;
        }

        #endregion

        #region Public Methods

        public void AddOrder(Order order)
        {
            _repository.Add(order);
        }

        public async Task ApproveOrderAsync(Guid orderId, string paymentProviderReference)
        {
            var order = await _repository.GetByIDAsync(orderId);
            if (order != null)
            {
                order.PaymentProviderReference = paymentProviderReference;
                _repository.Update(order);
            }
        }

        public Order AddNewOrder(OrderTypeEnum orderType, int amount, string currency, string reference, string userId, Guid? orderId)
        {
            var order = new Order
            {
                CreateTS = DateTime.UtcNow,
                OrderTypeId = (int) orderType,
                Amount = amount,
                Currency = currency,
                Reference = reference,
                UserId = userId,
                OrderId = orderId.HasValue? orderId.Value : new Guid(),
                Status = OrderStatus.Pending
            };
            _repository.Add(order);
            return order;
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            return await _repository.GetByIDAsync(orderId);
        }

        public async Task<Order> GetOrderAsync(string reference)
        {
            return await _repository.FirstOrDefaultAsync(o => o.Reference == reference);
        }

        public async Task<string> GetApprovalStatus(Guid orderId)
        {
            var order = await _repository.GetByIDAsync(orderId);

            if (order == null)
            {
                return ApprovalStatus.Pending;
            }

            if (String.IsNullOrEmpty(order.Status) || order.Status == OrderStatus.Pending)
            {
                (order.Status, order.PaymentProviderReference) = await _paymentService.GetPaymentStatusAsync(order.OrderId);
                _repository.Update(order);
            }
            if (order.Status == OrderStatus.Failed || order.Status == OrderStatus.Refund)
            {
                return ApprovalStatus.Declined;
            }
            if (order.Status == OrderStatus.Success)
            {
                return ApprovalStatus.Approved;
            }

            if (order.Status == OrderStatus.NotFound)
            {
                return ApprovalStatus.PaymentNotFound;
            }
            return ApprovalStatus.Pending;
        }

        #endregion
    }
}