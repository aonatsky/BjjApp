using System;
using System.Threading.Tasks;
using TRNMNT.Core.Enum;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// Gets the new order.
        /// </summary>
        /// <param name="orderType">Type of the order.</param>
        /// <param name="ammount">The ammount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        Order GetNewOrder(OrderTypeEnum orderType, int ammount, string currency, string reference);

        /// <summary>
        /// Approves the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="paymentProviderReference">The payment provider reference.</param>
        /// <returns></returns>
        Task ApproveOrderAsync(Guid orderId, string paymentProviderReference);

        /// <summary>
        /// Gets the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        Task<Order> GetOrderAsync(Guid orderId);

        /// <summary>
        /// Adds the order.
        /// </summary>
        /// <param name="order">The order.</param>
        void AddOrder(Order order);
    }
}
