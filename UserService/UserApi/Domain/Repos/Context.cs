using User = UserApi.Controllers.User;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Repos
{

    public class ReadContext : Context
    {
        public ReadContext() : base("ReadOnly")
        { }
    }

    public class WriteContext : Context
    {
        public WriteContext() : base("ReadWrite")
        { }
    }

    public class MigrateContext : Context
    {
        public MigrateContext() : base("Migrate")
        { }
    }

    public abstract class Context : DbContext
    {
        public Context(string connectionName)
            :base(connectionName)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            // this.Configuration.ProxyCreationEnabled = false;

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var userEntity = modelBuilder.Entity<User>();
            userEntity.ToTable("Users");
            userEntity.HasKey(u => u.Id);
            userEntity.Property(u => u.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            userEntity.HasIndex(u => u.Email).IsUnique();

            userEntity.Property(u => u.Email).IsRequired().HasMaxLength(254);
            userEntity.Property(u => u.FirstName).IsRequired();
            userEntity.Property(u => u.LastName).IsRequired();

            userEntity.Ignore(u => u.Password);
            userEntity.Property(u => u.Email).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}