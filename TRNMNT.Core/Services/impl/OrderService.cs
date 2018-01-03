using System;
using System.Threading.Tasks;
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
        private readonly IAppDbContext _unitOfWork;

        #endregion

        #region .ctor

        public OrderService(IRepository<Order> orderRepository, IAppDbContext unitOfWork)
        {
            _repository = orderRepository;
            _unitOfWork = unitOfWork;
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
                order.PaymentApproved = true;
                _repository.Update(order);
            }
        }

        public Order GetNewOrder(OrderTypeEnum orderType, int ammount, string currency, string reference)
        {
            var order = new Order
            {
                CreateTS = DateTime.UtcNow,
                OrderTypeId = (int)orderType,
                Amount = ammount,
                Currency = currency,
                PaymentApproved = false,
                Reference = reference
            };
            return order;
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            return await _repository.GetByIDAsync(orderId);
        }

        #endregion
    }
}
