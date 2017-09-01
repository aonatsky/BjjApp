using System;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public class OrderService : IOrderService
    {
        private IRepository<Order> orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task ApproveOrderAsync(Guid orderId)
        {
            var order = await orderRepository.GetByIDAsync(orderId);
            order.PaymentApproved = true;
            orderRepository.Update(order);
            await orderRepository.SaveAsync();
        }

        public async Task<Order> GetNewOrderAsync(OrderTypeEnum orderType, string userId, int ammount, string currency, string reference)
        {
            var order = new Order
            {
                UserId = userId,
                CreateTS = DateTime.UtcNow,
                OrderType = (int)orderType,
                Amount = ammount,
                Currency = currency,
                PaymentApproved = false,
                Reference = reference
            };
            orderRepository.Add(order);
            await orderRepository.SaveAsync();
            return order;
        }
    }
}
