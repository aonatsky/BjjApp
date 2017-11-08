using TRNMNT.Core.Services;

namespace TRNMNT.Core.Helpers
{
    public interface IPaidServiceFactory
    {
        IPaidEntityService GetService(int orderTypeId);
    }
}
