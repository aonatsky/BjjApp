using System;
using System.Threading.Tasks;

namespace TRNMNT.Core.Services.Interface
{
    public interface IPaidEntityService
    {
        /// <summary>
        /// Approves the entity asynchronous.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        Task ApproveEntityAsync(Guid entityId, Guid orderId);
    }
}
