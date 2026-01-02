using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Application.DTOs;
using RestaurantManagement.Application.Interfaces;
using RestaurantManagement.Application.Services;

namespace RestaurantManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(UserRoleDto dto)
        {
            var created = await _userRoleService.CreateUserRoleAsync(dto, currentUserId: 1);
            return Ok(created);
        }
    }
}
