
using InventorySystem.Data;
using InventorySystem.Dtos;
using InventorySystem.Entities;
using InventorySystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Services
{
    public class TenantService(
        TenantDbContext _context,
        IConfiguration _configuration,
        IServiceProvider _serviceProvider

    ) : ITenantService
    {
        public async Task<Tenant> CreateTenantAsync(TenantCreateDto tenantCreateDto)
        {
            string newConnectionString = null;

            if (tenantCreateDto.Isolated == true)
            {
                string dbName = "inventorytenant-" + tenantCreateDto.Id;
                string defaultconnectionString = _configuration.GetConnectionString("DefaultConnection");
                newConnectionString = defaultconnectionString.Replace("inventorytenant", dbName);

                try
                {
                    using IServiceScope serviceScope = _serviceProvider.CreateScope();
                    ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    applicationDbContext.Database.SetConnectionString(newConnectionString);

                    if ((await applicationDbContext.Database.GetPendingMigrationsAsync()).Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Applying application migrations for new '{tenantCreateDto.Id}'");
                        Console.ResetColor();
                        await applicationDbContext.Database.MigrateAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            Tenant tenant = new()
            {
                Id = tenantCreateDto.Id,
                Name = tenantCreateDto.Name,
                ConnectionString = newConnectionString
            };

            await _context.AddAsync(tenant);
            await _context.SaveChangesAsync();

            return tenant;
        }

        public async Task<ICollection<Tenant>> GetAllTenantsAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant> GetTenantByIdAsync(string tenantId)
        {
            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId);
            if (tenant == null)
            {
                throw new Exception($"Tenant with id {tenantId} not found");
            }
            return tenant;
        }

        public async Task<bool> TenantExitsAsync(string tenantId)
        {
            return await _context.Tenants.AnyAsync(x => x.Id == tenantId);
        }

        public Task<bool> DeleteTenantAsyc(string tenantId)
        {
            throw new NotImplementedException();
        }
    }
}