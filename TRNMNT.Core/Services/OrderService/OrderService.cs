using System;
using System.Threading.Tasks;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.UnitOfWork;
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

        public async Task AddOrderAsync(Order order, bool saveContext = true)
        {
            orderRepository.Add(order);
            if (saveContext)
            {
                await unitOfWork.SaveAsync();
            }

        }

        public async Task ApproveOrderAsync(Guid orderId, bool saveContext = true)
        {
            var order = await orderRepository.GetByIDAsync(orderId);
            order.PaymentApproved = true;
            orderRepository.Update(order);
            if (saveContext)
            {
                await unitOfWork.SaveAsync();
            }
        }

        public Order GetNewOrder(OrderTypeEnum orderType, string userId, int ammount, string currency, string reference)
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
            return order;
        }
    }
}
