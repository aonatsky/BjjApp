using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Data.Entities;

namespace TRNMNT.Core.Data
{
        public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Owner>().HasKey(o => o.OwnerId);
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("trnmnt");
            
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        #region interface Implementation
        public new IQueryable<T> Set<T>() where T : class
        {
            return SetInternal<T>().AsQueryable();
        }
        public void Add<T>(T entity) where T : class
        {
            SetInternal<T>().Add(entity);
        }
        public void Modify<T>(T entity) where T : class
        {
            if (entity != null)
            {
                if (Entry(entity).State != EntityState.Modified)
                {
                    Entry(entity).State = EntityState.Modified;
                }
            }
        }
        public void Remove<T>(T entity) where T : class
        {
            SetInternal<T>().Remove(entity);
        }
        public bool Save(bool suppressExceptions = true)
        {
            try
            {
                base.SaveChanges();
                return true;
            }
            catch (System.Exception ex)
            {
                if (!suppressExceptions)
                {
                    throw ex;
                }
                return false;
            }
        }


        public async Task<int> SaveAsync(bool suppressExceptions)
        {
            try
            {
               return await base.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!suppressExceptions)
                {
                    throw ex;
                }
                return 0;
            }
        }
        
        #endregion


        protected DbSet<T> SetInternal<T>() where T : class
        {
            return base.Set<T>();
        }

       

        public DbSet<Owner> Owner { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<TournamentType> TournamentType { get; set; }
        public DbSet<WeightDivision> WeightDivision { get; set; }
        public DbSet<BeltDivision> BeltDivision { get; set; }
        public DbSet<Fighter> Fighter { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Fight> Fight { get; set; }
        public DbSet<FightList> FightList { get; set; }
        public DbSet<AgeDivision> AgeDivision {get;set;}
        
    }
}