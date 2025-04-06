using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventorySystem.Data
{
    public class DesignTimeTenantDbcontextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<TenantDbContext> optionsBuilder = new();
            _ = optionsBuilder.UseSqlServer(connectionString);

            return new TenantDbContext(optionsBuilder.Options);
        }
    }
}