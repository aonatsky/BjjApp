using System;
using System.Threading.Tasks;
using TRNMNT.Core.Enum;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IOrderService
    {
        Order GetNewOrder(OrderTypeEnum orderType, int ammount, string currency, string reference);
        Task ApproveOrderAsync(Guid orderId, string paymentProviderReference);
        Task<Order> GetOrder(Guid orderId);
        void AddOrder(Order order);
    }
}
