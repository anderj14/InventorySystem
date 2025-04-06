
using InventorySystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Data
{
    public class TenantDbContext: DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
    }
}