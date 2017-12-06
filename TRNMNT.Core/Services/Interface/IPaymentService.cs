using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IPaymentService
    {
        PaymentDataModel GetPaymentDataModel(Order order, string callbackUrl);
        Task ConfirmPaymentAsync(PaymentDataModel dataModel);
    }
}
