using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TRNMNT.Data.Context
{
    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Modify<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        bool Save(bool suppressExceptions = true);
        void AddRange(IEnumerable<object> entities);
        Task<int> SaveAsync(bool suppressExceptions = false);
        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
