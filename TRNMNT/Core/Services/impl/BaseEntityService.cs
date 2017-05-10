using System;
using System.Collections.Generic;
using System.Linq;
using TRNMNT.Core.Data;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;

namespace TRNMNT.Core.Services.impl
{
    public class BaseEntityService<T> where T : class
    {
        IRepository<T> repository;

        public BaseEntityService(IRepository<T> repository)
        {
            this.repository = repository;
        }


        public T GetByID<TKey>(TKey id)
        {
            return repository.GetByID<TKey>(id);
        }
        public IQueryable<T> GetAll()
        {
            return repository.GetAll();
        }
        void Add(T entity)
        {
            repository.Add(entity);
        }
        void Update(T entity)
        {
            repository.Update(entity);
        }

        void AddRange(IEnumerable<T> entities)
        {
            repository.AddRange(entities);
        }
        void Delete(T entity)
        {
            repository.Delete(entity);
        }
        void Save(bool supressExceptions = true)
        {
            repository.Save();
        }

               
    }
}
