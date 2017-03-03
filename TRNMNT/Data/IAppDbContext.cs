using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Entities;

namespace TRNMNT.Data
{
    public interface IAppDbContext 
    {
        IQueryable<T> Set<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Modify<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;        
        bool Save(bool suppressExceptions = true);
        Task<int> SaveAsync(bool suppressExceptions = true);

        //DBSets
         DbSet<Owner> Owner { get; set; }
         DbSet<Tournament> Tournament { get; set; }
         DbSet<TournamentType> TournamentType { get; set; }
         DbSet<WeightClass> WeightClass { get; set; }
         DbSet<BeltClass> BeltClass { get; set; }
         DbSet<Fighter> Fighter { get; set; }
         DbSet<Team> Team { get; set; }
         DbSet<Fight> Fight { get; set; }
         DbSet<FightList> FightList { get; set; }
         DbSet<AgeClass> AgeClass {get;set;}
    }
}
