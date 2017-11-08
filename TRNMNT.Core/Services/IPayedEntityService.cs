using System;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services
{
    public interface IPayedEntityService
    {
        Task ApproveEntity(Guid entityId, Guid orderId);

    }
}
