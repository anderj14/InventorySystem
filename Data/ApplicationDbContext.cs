
using InventorySystem.Entities;
using InventorySystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ITenantContextService _tenantContextService;
        public string ActivateTenantId { get; set; }
        public string ActivateTenantConnectionString { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContextService tenantContextService)
            : base(options)
        {
            _tenantContextService = tenantContextService;
            ActivateTenantId = _tenantContextService.TenantId;
            ActivateTenantConnectionString = _tenantContextService.ConnectionString;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.TenantId == ActivateTenantId);

            modelBuilder.Entity<Product>()
            .Property(p => p.UnitPrice)
            .HasColumnType("decimal(18,2)");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string tenantConnectionString = ActivateTenantConnectionString;
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                _ = optionsBuilder.UseSqlServer(tenantConnectionString);
            }
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = ActivateTenantId;
                        break;
                }
            }
            return base.SaveChanges();
        }

    }
}