using AutoMapper;
using RestaurantManagement.Application.DTOs;
using RestaurantManagement.Application.Interfaces;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IAsyncRepository<UserRole> _repo;

        private readonly IMapper _mapper;

        public UserRoleService(IAsyncRepository<UserRole> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<UserRoleDto> CreateUserRoleAsync(UserRoleDto dto, int currentUserId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserRoleDto>> GetUserRolesAsync(int userId)
        {
            // Only filter by userId from controller — IsActive is enforced here
            var roles = await _repo.ListAsync(ur => ur.UserId == userId && ur.IsActive);
            return _mapper.Map<IEnumerable<UserRoleDto>>(roles);
        }

    }

}
