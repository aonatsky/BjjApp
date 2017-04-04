using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Data.Fake
{
        public class FakeAppDbContext : IAppDbContext
    {
        public FakeAppDbContext(DbContextOptions<AppDbContext> options)
        {
            
        }


        #region interface Implementation
        public new IQueryable<T> Set<T>() where T : class
        {
            throw new NotImplementedException();
        }
        public void Add<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
        public void Modify<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
        public void Remove<T>(T entity) where T : class
        {
            SetInternal<T>().Remove(entity);
        }
        public bool Save(bool suppressExceptions = true)
        {
            throw new NotImplementedException();
        }


        public async Task<int> SaveAsync(bool suppressExceptions)
        {
            throw new NotImplementedException();
        }
        
        #endregion


        protected DbSet<T> SetInternal<T>() where T : class
        {
            throw new NotImplementedException();
        }

        DbSet<T> IAppDbContext.Set<T>()
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<T> Entry<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public DbSet<Fighter> Fighter { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<WeightDivision> WeightDivision { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<Category> Category { get; set; }
        // public DbSet<Tournament> Tournament { get; set; }
        // public DbSet<TournamentType> TournamentType { get; set; }
        
        // public DbSet<BeltDivision> BeltDivision { get; set; }
        
        // public DbSet<Fight> Fight { get; set; }
        // public DbSet<FightList> FightList { get; set; }
        // public DbSet<AgeDivision> AgeDivision {get;set;}
    }
}
