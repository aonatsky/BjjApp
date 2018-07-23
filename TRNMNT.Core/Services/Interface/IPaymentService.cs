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
        /// <param name="callbackUrl">The callback URL.</param>
        /// <returns></returns>
        PaymentDataModel GetPaymentDataModel(Order order, string callbackUrl);

        /// <summary>
        /// Confirms the payment asynchronous.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <returns></returns>
        Task ConfirmPaymentAsync(PaymentDataModel dataModel);

        PaymentDataModel CheckStatusAsync(string orderId);
    }
}
