using RestaurantManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> ValidateUserAsync(string username, string password);
        Task<IEnumerable<MenuItemDto>> GetUserMenuAsync(int userId);
        Task<UserDto> CreateUserAsync(UserDto dto, int currentUserId);
        Task<UserDto> UpdateUserAsync(UserDto dto, int currentUserId);
        Task<IEnumerable<UserDto>> GetAllAsync();

        // ✅ New method
        Task<IEnumerable<UserDto>> GetAllCreatedByAsync(int creatorUserId);


    }
}
