using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRNMNT.Data.Repositories
{
    public interface IRepository<T> where T:class  
    {
        T GetByID<TKey>(TKey id);
        Task<T> GetByIDAsync<TKey>(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete (T entity);
        void DeleteRange(IEnumerable<T> entity);
        void Delete<TKey>(TKey id);
    }
}
