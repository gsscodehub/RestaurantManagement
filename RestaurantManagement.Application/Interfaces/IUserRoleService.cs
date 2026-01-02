using RestaurantManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleDto>> GetUserRolesAsync(int userId);
        Task<UserRoleDto> CreateUserRoleAsync(UserRoleDto dto, int currentUserId);

    }
}
