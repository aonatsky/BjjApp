using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TRNMNT.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        bool Save(bool supressExceptions = false);
        Task<int> SaveAsync(bool suppressExceptions = false);
    }
}
