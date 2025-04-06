using InventorySystem.Dtos;
using InventorySystem.Entities;
using InventorySystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    // [Route("[controller]")]
    public class TenantsController(ITenantService _tenantService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            ICollection<Tenant> tenants = await _tenantService.GetAllTenantsAsync();
            return View(tenants);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TenantCreateDto tenantCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Model");

            try
            {
                if (await _tenantService.TenantExitsAsync(tenantCreateDto.Id))
                {
                    ModelState.AddModelError(string.Empty, $"Tenant '{tenantCreateDto.Name}' already exists.");
                    return View(tenantCreateDto);
                }
                var tenant = await _tenantService.CreateTenantAsync(tenantCreateDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the tenant.");
                return StatusCode(500, new { error = ex.Message });
            }

        }

        // [HttpGet("{tenantId}")]
        // public async Task<IActionResult> GetTenantById(string tenantId)
        // {
        //     try
        //     {
        //         var tenant = await _tenantService.GetTenantByIdAsync(tenantId);

        //         return Ok(tenant);
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(new { error = ex.Message });
        //     }
        // }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

    }
}