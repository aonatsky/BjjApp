using System;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IPaymentService
    {
        /// <summary>
        /// Gets the payment data model.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns></returns>
        PaymentDataModel GetPaymentDataModel(Order order, string serverUrl, string redirectUrl);

        /// <summary>
        /// Confirms the payment asynchronous.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <returns></returns>
        Task ConfirmPaymentAsync(PaymentDataModel dataModel);
        
        /// <summary>
        /// Returns order status and provider reference
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns></returns>
        Task<(string status, string paymentProviderReference)> GetPaymentStatusAsync(Guid orderId);
    }
}