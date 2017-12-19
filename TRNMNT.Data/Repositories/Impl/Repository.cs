using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Context;

namespace TRNMNT.Data.Repositories.Impl
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

        public async Task<T> GetByIDAsync<K>(K id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            context.Modify(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            context.AddRange(entities);
        }

        public void Save(bool supressExceptions = true)
        {
            context.Save(supressExceptions);
        }

        public void Delete(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public void Delete<K>(K id)
        {
            var entity = GetByID<K>(id);
            context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public async Task SaveAsync(bool supressExceptions = true)
        {
            await context.SaveAsync(supressExceptions);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entitity in entities)
            {
                Delete(entitity);
            }
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            var queryable = GetAll();
            foreach (var includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }
    }
}