using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class FederationService : IFederationService
    {
        #region Dependencies

        private readonly IRepository<Federation> _repository;

        #endregion

        #region .ctor

        public FederationService(IRepository<Federation> federationRepository)
        {
            _repository = federationRepository;
        }

        #endregion

        #region public methods
        
        #endregion

    }
}