using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Helpers.Interface
{
    public interface IPaidServiceFactory
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="orderTypeId">The order type identifier.</param>
        /// <returns></returns>
        IPaidEntityService GetService(int orderTypeId);
    }
}
