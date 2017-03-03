using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Entities;

namespace TRNMNT.Data.Fake
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

       

           public DbSet<Owner> Owner { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<TournamentType> TournamentType { get; set; }
        public DbSet<WeightClass> WeightClass { get; set; }
        public DbSet<BeltClass> BeltClass { get; set; }
        public DbSet<Fighter> Fighter { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Fight> Fight { get; set; }
        public DbSet<FightList> FightList { get; set; }
        public DbSet<AgeClass> AgeClass {get;set;}

        


    }
}
