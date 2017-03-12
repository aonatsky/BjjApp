using System.Linq;
using System.Collections.Generic;

namespace TRNMNT.Core.Data.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private IAppDbContext context;
        private IQueryable<T> entities;
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

        public T GetByID<Guid>(Guid id)
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

    }
}