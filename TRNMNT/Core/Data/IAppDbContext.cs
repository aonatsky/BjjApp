using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TRNMNT.Core.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TRNMNT.Core.Data
{
    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Modify<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        bool Save(bool suppressExceptions = true);

        void AddRange(IEnumerable<object> entities);
        Task<int> SaveAsync(bool suppressExceptions = true);

        EntityEntry<T> Entry<T>(T entity) where T : class;

        //DBSets
        DbSet<WeightDivision> WeightDivision { get; set; }
        //DbSet<BeltDivision> BeltDivision { get; set; }
        DbSet<Fighter> Fighter { get; set; }
        DbSet<Team> Team { get; set; }
        DbSet<Category> Category { get; set; }

        //DbSet<Owner> Owner { get; set; }
        //DbSet<Tournament> Tournament { get; set; }
        //DbSet<TournamentType> TournamentType { get; set; }
        //DbSet<Fight> Fight { get; set; }
        //DbSet<FightList> FightList { get; set; }
        //DbSet<AgeDivision> AgeDivision {get;set;}
    }
}
