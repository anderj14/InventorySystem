
namespace InventorySystem.Dtos
{
    public class TenantCreateDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public bool Isolated { get; set; }
    }
}