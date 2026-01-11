using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RestaurantManagement.Application.DTOs;
using RestaurantManagement.Application.Interfaces;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Interfaces.Repositories;
using RestaurantManagement.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository<User> _repo; // Hybrid repository
        private readonly IMapper _mapper;

        public UserService(IAsyncRepository<User> repo,  IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserDto?> ValidateUserAsync(string username, string password)
        {
            // var passwordHash = Hash(password);

            var user = await _repo.ValidateUserSpAsync(username, password);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }


        public async Task<IEnumerable<MenuItemDto>> GetUserMenuAsync(int userId)
        {
            var pages = await _repo.GetUserMenuBySpAsync(userId);
            return _mapper.Map<IEnumerable<MenuItemDto>>(pages);

        }
        public async Task<UserDto> CreateUserAsync(UserDto dto, int currentUserId)
        {
            var entity = _mapper.Map<User>(dto);
            //entity.PasswordHash = Hash("Default@123"); // demo only
            entity.MarkCreated(currentUserId);
            await _repo.AddAsync(entity);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> UpdateUserAsync(UserDto dto, int currentUserId)
        {
            var entity = _mapper.Map<User>(dto);
            entity.MarkCreated(currentUserId);
            await _repo.UpdateAsync(entity);
            return _mapper.Map<UserDto>(entity);
        }
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.ListAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<UserDto>> GetAllCreatedByAsync(int userId)
        {
            var users = await _repo.ListCreatedByAsync(userId);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }


        private static string Hash(string input)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: new byte[] { 1, 2, 3, 4 },
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
        }
    }
}
