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
    public class RoleService : IRoleService
    {
        private readonly IAsyncRepository<Role> _repo;
        private readonly IMapper _mapper;

        public RoleService(IAsyncRepository<Role> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto dto, int currentUserId)
        {
            var entity = _mapper.Map<Role>(dto);
            entity.MarkCreated(currentUserId);
            await _repo.AddAsync(entity);
            return _mapper.Map<RoleDto>(entity);
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _repo.ListAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<IEnumerable<RoleDto>> GetActiveRolesAsync()
        {           
                var roles = await _repo.GetActiveRolesBySpAsync();
                return _mapper.Map<IEnumerable<RoleDto>>(roles);            
        }
    }
}
