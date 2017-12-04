using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services
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
