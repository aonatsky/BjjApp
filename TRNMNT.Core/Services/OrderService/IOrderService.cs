using System;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public interface IOrderService
    {
        Order GetNewOrder(OrderTypeEnum orderType, string userId, int ammount, string currency, string reference);
        Task ApproveOrderAsync(Guid orderId, bool saveContext = true);
        void AddOrder(Order order);
    }
}
