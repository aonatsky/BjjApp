using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class FederationService : IFederationService
    {
        private readonly IRepository<Federation> repository;

        public FederationService(IRepository<Federation> federationRepository)
        {
            this.repository = federationRepository;
        }
    }
}
