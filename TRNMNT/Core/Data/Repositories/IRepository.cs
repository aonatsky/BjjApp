using System.Collections.Generic;
using System.Linq;

namespace TRNMNT.Core.Data.Repositories
{
    public interface IRepository<T> where T:class  
    {
        T GetByID<TKey>(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete (T entity);
        void Save(bool supressExceptions = true);
    }
}
