using System;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Web.Core.Enum;

namespace TRNMNT.Core.Services
{
    public interface IOrderService
    {
        Task<Order> GetNewOrderAsync(OrderTypeEnum orderType, string userId, int ammount, string currency, string reference);
        Task ApproveOrderAsync(Guid orderId);
    }
}
