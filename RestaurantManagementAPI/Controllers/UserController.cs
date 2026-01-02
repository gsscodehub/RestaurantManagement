using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Application.DTOs;
using RestaurantManagement.Application.Interfaces;

namespace RestaurantManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) => _userService = userService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            var created = await _userService.CreateUserAsync(dto, currentUserId: 1);
            return Ok(created);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UserDto dto)
        {
            var created = await _userService.UpdateUserAsync(dto, currentUserId: 1);
            return Ok(created);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
    }
}
