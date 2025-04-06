
using InventorySystem.Dtos;
using InventorySystem.Entities;

namespace InventorySystem.Interfaces
{
    public interface ITenantService
    {
        Task<Tenant> CreateTenantAsync(TenantCreateDto tenantCreateDto);
        Task<ICollection<Tenant>> GetAllTenantsAsync();
        Task<Tenant> GetTenantByIdAsync(string tenantId);
        Task<bool> DeleteTenantAsyc(string tenantId);
        Task<bool> TenantExitsAsync(string tenantId);
    }
}