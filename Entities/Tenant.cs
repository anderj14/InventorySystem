
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Entities
{
    public class Tenant
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ConnectionString { get; set; }
    }
}