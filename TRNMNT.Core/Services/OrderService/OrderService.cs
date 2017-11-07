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
        private IRepository<Order> orderRepository;
        private IAppDbContext unitOfWork;

        public OrderService(IRepository<Order> orderRepository, IAppDbContext unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public void AddOrder(Order order)
        {
            orderRepository.Add(order);
        }

        public async Task ApproveOrderAsync(Guid orderId, string paymentProviderReference)
        {
            var order = await orderRepository.GetByIDAsync(orderId);
            order.PaymentProviderReference = paymentProviderReference;
            order.PaymentApproved = true;
            orderRepository.Update(order);
        }

        public Order GetNewOrder(OrderTypeEnum orderType, int ammount, string currency, string reference)
        {
            var order = new Order
            {
                CreateTS = DateTime.UtcNow,
                OrderType = (int)orderType,
                Amount = ammount,
                Currency = currency,
                PaymentApproved = false,
                Reference = reference
            };
            return order;
        }
    }
}
