using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Context;

namespace TRNMNT.Data.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private IAppDbContext context;
        string errorMessage = string.Empty;
        public Repository(IAppDbContext context)
        {
            this.context = context;
        }
        public void Add(T entity)
        {
            context.Add(entity);
        }

        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T GetByID<K>(K id)
        {
            return context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            context.Modify(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            context.AddRange(entities);
        }

        public void Save(bool supressExceptions = true){
            context.Save(supressExceptions);
        }

        public void Delete(T entity){
            context.Entry<T>(entity).State = EntityState.Deleted;
        }
        
        public void Delete<K>(K id) 
        {
            var entity = GetByID<K>(id);
            context.Entry<T>(entity).State = EntityState.Deleted;
        }
    }
}