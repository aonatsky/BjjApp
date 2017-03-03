using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRNMNT.Data.Repositories
{
    interface IRepository<T> where T:class  
    {
        T GetByID<TKey>(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
    }
}
