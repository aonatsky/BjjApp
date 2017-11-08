using System;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services
{
    public interface IPaidEntityService
    {
        Task ApproveEntityAsync(Guid entityId, Guid orderId);

    }
}
