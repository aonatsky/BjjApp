using System;
using System.Threading.Tasks;
using TRNMNT.Core.Enum;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// Adds new order.
        /// </summary>
        /// <param name="orderType">Type of the order.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="reference">The reference.</param>
        /// <param name="userId">User id.</param>
        /// <returns></returns>
        Order AddNewOrder(OrderTypeEnum orderType, int amount, string currency, string reference, string userId);

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
    }
}
