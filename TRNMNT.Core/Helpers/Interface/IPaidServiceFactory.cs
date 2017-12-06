using TRNMNT.Core.Services.Interface;

namespace TRNMNT.Core.Helpers.Interface
{
    public interface IPaidServiceFactory
    {
        IPaidEntityService GetService(int orderTypeId);
    }
}
