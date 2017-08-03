using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TRNMNT.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TRNMNT.Data.Context
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
        DbSet<Fighter> Fighter { get; set; }
        DbSet<Team> Team { get; set; }
        DbSet<WeightDivision> Category { get; set; }
        DbSet<User> User{ get; set; }
        DbSet<Event> Event { get; set; }
        DbSet<Participant> Participant{ get; set; }

    }
}
