using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // EF CRUD
        public async Task<T?> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> ListAllAsync() => await _dbContext.Set<T>().ToListAsync();
        public async Task AddAsync(T entity) { await _dbContext.AddAsync(entity); await _dbContext.SaveChangesAsync(); }
        public async Task UpdateAsync(T entity) { _dbContext.Update(entity); await _dbContext.SaveChangesAsync(); }
        public async Task DeleteAsync(T entity) { _dbContext.Remove(entity); await _dbContext.SaveChangesAsync(); }

        // Generic SP placeholders (optional)
        public async Task<T?> GetLastRecordAsync() => await _dbContext.Set<T>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        public async Task<bool> CheckIfRecordExists(string name, decimal percentage) => await Task.FromResult(false);
        public DataTable GetSalesOrderItemsBySOId(int id) => new DataTable();

        // Concrete SP methods for User
        public async Task<IEnumerable<Page>> GetUserMenuBySpAsync(int userId)
        {
            using var conn = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();
            return await conn.QueryAsync<Page>(
                "sp_GetUserMenuRights",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }

        // Concrete SP methods for Role
        public async Task<IEnumerable<Role>> GetActiveRolesBySpAsync()
        {
            using var conn = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();
            return await conn.QueryAsync<Role>(
                "sp_GetActiveRoles",
                commandType: CommandType.StoredProcedure);
        }

        Task<T?> IAsyncRepository<T>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<T?> IAsyncRepository<T>.GetLastRecordAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> ValidateUserSpAsync(string username, string passwordHash)
        {
            using var conn = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);
            await conn.OpenAsync();

            return await conn.QueryFirstOrDefaultAsync<User>(
                "sp_ValidateUser",
                new
                {
                    Username = username,
                    PasswordHash = passwordHash
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate) 
        { 
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
