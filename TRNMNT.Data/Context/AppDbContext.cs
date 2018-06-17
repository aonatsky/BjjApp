using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TRNMNT.Data.Entities;

namespace TRNMNT.Data.Context
{
    public class AppDbContext : IdentityDbContext<User>, IAppDbContext
    {
        #region .ctor
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }
        #endregion

        #region protected
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Owner>().HasKey(o => o.OwnerId);
            base.OnModelCreating(builder);
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            builder.Entity<Participant>()
                .HasOne(m => m.WeightDivision)
                .WithMany(t => t.Participants)
                .HasForeignKey(m => m.WeightDivisionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Participant>()
                .HasOne(m => m.AbsoluteWeightDivision)
                .WithMany(t => t.AbsoluteDivisionParticipants)
                .HasForeignKey(m => m.AbsoluteWeightDivisionId)
                .OnDelete(DeleteBehavior.Restrict);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        #endregion


        #region interface implementation
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

        EntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
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

        #region dbSets
        protected DbSet<T> SetInternal<T>() where T : class
        {
            return base.Set<T>();
        }


        public DbSet<WeightDivision> WeightDivision { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Federation> Federation { get; set; }
        public DbSet<PromoCode> PromoCode { get; set; }
        public DbSet<Match> Round { get; set; }

        #endregion
    }
}
