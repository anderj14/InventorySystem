
using InventorySystem.Data;
using InventorySystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Extensions
{
    public static class MultitenantMigrationHelper
    {
        public static IServiceCollection ApplyTenantMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
            TenantDbContext tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbContext>();
            // Apply migrations for the tenant database
            if (tenantDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Applying pending migrations for tenant database...");
                Console.ResetColor();
                tenantDbContext.Database.Migrate();
            }

            // Retrieve all tenants from the database
            List<Tenant> tenantsList = tenantDbContext.Tenants.ToList();
            string defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

            // Loop through each tenant and apply migrations
            foreach (Tenant tenant in tenantsList)
            {
                string connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString;

                using IServiceScope tenantScope = services.BuildServiceProvider().CreateScope();
                ApplicationDbContext applicationDbContext = tenantScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // dynamically set the connection string for the tenant database
                applicationDbContext.Database.SetConnectionString(connectionString);

                // Apply migrations for the tenant database
                if (applicationDbContext.Database.GetPendingMigrations().Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Applying pending migrations for tenant database {tenant.Id}...");
                    Console.ResetColor();
                    applicationDbContext.Database.Migrate();
                }

            }

            return services;
        }
    }
}