using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Application.DTOs;
using RestaurantManagement.Application.Interfaces;

namespace RestaurantManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService) => _roleService = roleService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(RoleDto dto)
        {
            var created = await _roleService.CreateRoleAsync(dto, currentUserId: 1);
            return Ok(created);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRoles()
        {
            var roles = await _roleService.GetActiveRolesAsync();
            return Ok(roles);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateRole(RoleDto dto)
        {
            var updated = await _roleService.UpdateRoleAsync(dto, currentUserId: 1);
            return Ok(updated);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id, currentUserId: 1);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return Ok(role);
        }

    }
}
