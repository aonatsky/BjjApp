
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Data.Repositories
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
             throw new NotImplementedException();
        }

        public T GetByID<Guid>(Guid id)
        {
            Console.WriteLine($"Entity with id {id} is called");
             throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            Console.WriteLine($"Entity {entity.GetType()} is updated");
        }

        public void AddRange(IEnumerable<T> entities)
        {
            Console.WriteLine($"Entities are added");
        }

    }
}