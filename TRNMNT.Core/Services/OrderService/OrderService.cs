using System;
using System.Threading.Tasks;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public class OrderService : IOrderService
    {
        private IRepository<Order> repository;
        private IAppDbContext unitOfWork;

        public OrderService(IRepository<Order> orderRepository, IAppDbContext unitOfWork)
        {
            this.repository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public void AddOrder(Order order)
        {
            repository.Add(order);
        }

        public async Task ApproveOrderAsync(Guid orderId, string paymentProviderReference)
        {
            var order = await repository.GetByIDAsync(orderId);
            if (order != null)
            {
                order.PaymentProviderReference = paymentProviderReference;
                order.PaymentApproved = true;
                repository.Update(order);
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

        public async Task<Order> GetOrder(Guid orderId)
        {
            return await repository.GetByIDAsync(orderId);
        }
    }
}
