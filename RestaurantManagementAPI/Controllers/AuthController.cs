using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Application.Interfaces;
using RestaurantManagement.Application.Services;
using RestaurantManagement.Domain.Interfaces.Security;

namespace RestaurantManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRolService;
        private readonly IJwtTokenGenerator _jwt;

        public AuthController(IUserService userService, IUserRoleService userRolService, IJwtTokenGenerator jwt)
        {
            _userService = userService;
            _userRolService = userRolService;
            _jwt = jwt;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.ValidateUserAsync(request.Username, request.Password);
            if (user == null) return Unauthorized();

            var menus = await _userService.GetUserMenuAsync(user.Id);

            // Build permission claims from menu rights
            var permissions = new List<string>();
            foreach (var m in menus)
            {
                if (m.CanView) permissions.Add($"{m.PageUrl}:view");
                if (m.CanCreate) permissions.Add($"{m.PageUrl}:create");
                if (m.CanEdit) permissions.Add($"{m.PageUrl}:edit");
                if (m.CanDelete) permissions.Add($"{m.PageUrl}:delete");
                if (m.CanUpdate) permissions.Add($"{m.PageUrl}:update");
            }
            // 🔑 Fetch role if assigned
            var roles = await _userRolService.GetUserRolesAsync(user.Id); 

            var token = _jwt.Generate(user.Id, permissions, roles.Select(r => r.RoleId.ToString()));

            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true, // not accessible via JavaScript
                Secure = true, // only over HTTPS
              //  SameSite = SameSiteMode.Strict, // prevent CSRF and this way third party can not have access of token like : react frontend code will nt receive the token set in browser
                SameSite = SameSiteMode.None, // ✅ allow cross-origin as this way when you get token then react will have token as well as swagger and token also work when token manually passed from swagger or postman or any third party app
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new
            {
                message = "Login successful",
                token,
                menu = menus,
                user,
                roles, // optional: return role info in response
                claims = permissions  // each page has 5 claims like : create/edit/update/view/delete
            });
        }

        public record LoginRequest(string Username, string Password);
    }
}
