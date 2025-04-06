
namespace InventorySystem.Interfaces
{
    public interface ITenantContextService
    {
        string? TenantId { get; set; }
        public string? ConnectionString { get; set; }
        public Task<bool> SetTenantAsync(string tenant);
    }
}