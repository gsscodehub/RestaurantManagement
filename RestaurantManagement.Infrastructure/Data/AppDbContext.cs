using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;

namespace RestaurantManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Apply configuration to all entities inheriting BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property<bool>(nameof(BaseEntity.IsActive))
                        .HasColumnName("IsActive")
                        .HasDefaultValue(true); // ✅ DB default true
                }
            }
        }
    }
}
