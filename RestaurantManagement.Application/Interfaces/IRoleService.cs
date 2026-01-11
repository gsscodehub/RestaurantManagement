using RestaurantManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> CreateRoleAsync(RoleDto dto, int currentUserId);
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<IEnumerable<RoleDto>> GetActiveRolesAsync();
        Task<RoleDto> UpdateRoleAsync(RoleDto dto, int currentUserId);
        Task<bool> DeleteRoleAsync(int id, int currentUserId);
        Task<RoleDto> GetByIdAsync(int id);
    }
}
