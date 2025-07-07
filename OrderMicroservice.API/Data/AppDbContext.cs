using Microsoft.EntityFrameworkCore;
using OrderMicroservice.API.Models;
namespace OrderMicroservice.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order!)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Reference)
                .IsUnique();
        }
        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is OrderItem)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedAt").CurrentValue = utcNow;
                        entry.Property("UpdatedAt").CurrentValue = utcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property("UpdatedAt").CurrentValue = utcNow;
                    }
                }
            }

            return base.SaveChangesAsync(ct);
        }

    }
}
