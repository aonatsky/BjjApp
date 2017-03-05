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

        DbSet<T> IAppDbContext.Set<T>()
        {
            throw new NotImplementedException();
        }

        void IAppDbContext.Add<T>(T entity)
        {
            throw new NotImplementedException();
        }

        void IAppDbContext.Modify<T>(T entity)
        {
            throw new NotImplementedException();
        }

        void IAppDbContext.Remove<T>(T entity)
        {
            throw new NotImplementedException();
        }

        bool IAppDbContext.Save(bool suppressExceptions)
        {
            throw new NotImplementedException();
        }

        void IAppDbContext.AddRange(IEnumerable<object> entities)
        {
            throw new NotImplementedException();
        }

        Task<int> IAppDbContext.SaveAsync(bool suppressExceptions)
        {
            throw new NotImplementedException();
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

        DbSet<Owner> IAppDbContext.Owner
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<Tournament> IAppDbContext.Tournament
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<TournamentType> IAppDbContext.TournamentType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<WeightDivision> IAppDbContext.WeightDivision
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<BeltDivision> IAppDbContext.BeltDivision
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<Fighter> IAppDbContext.Fighter
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<Team> IAppDbContext.Team
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<Fight> IAppDbContext.Fight
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<FightList> IAppDbContext.FightList
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DbSet<AgeDivision> IAppDbContext.AgeDivision
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
