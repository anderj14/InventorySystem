
using InventorySystem.Interfaces;

namespace InventorySystem.Entities
{
    public class Product: BaseEntity, ITenantEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ImageUrl { get; set; }
        public required string TenantId { get; set; }
    }
}