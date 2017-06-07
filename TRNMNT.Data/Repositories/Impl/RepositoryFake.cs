
using System;
using System.Linq;
using System.Collections.Generic;

namespace TRNMNT.Data.Repositories
{

    public class RepositoryFake<T> : IRepository<T> where T : class
    {
        private IQueryable<T> entities;
        string errorMessage = string.Empty;
        public RepositoryFake()
        {
        }
        public void Add(T entity)
        {
            Console.WriteLine($"Entity {entity.GetType()} is added");
        }

        public IQueryable<T> GetAll()
        {
             Console.WriteLine($"Get all is called");
             return new List<T>().AsQueryable();
        }

        public T GetByID<Guid>(Guid id)
        {
            Console.WriteLine($"Entity with id {id} is called");
            return null;
        }

        public void Update(T entity)
        {
            Console.WriteLine($"Entity {entity.GetType()} is updated");
        }

        public void AddRange(IEnumerable<T> entities)
        {
            Console.WriteLine($"Entities {typeof(T)} are added");
        }

        public void Save(bool supressExceptions){
            Console.WriteLine($"Saved");
        }

        public void Delete(T entity)
        {
            Console.WriteLine($"Entities {typeof(T)} are delete");
        }

        public void Delete<TKey>(TKey id)
        {
            throw new NotImplementedException();
        }
    }
}