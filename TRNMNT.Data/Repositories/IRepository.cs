using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TRNMNT.Data.Repositories
{
    public interface IRepository<T> where T:class  
    {
        T GetByID<TKey>(TKey id);
        Task<T> GetByIDAsync<TKey>(TKey id);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        void UpdateValues(T entity, T model);
        void Add(T entity);
        void Update(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete (T entity);
        void DeleteRange(IEnumerable<T> entity);
        void Delete<TKey>(TKey id);
    }
}
