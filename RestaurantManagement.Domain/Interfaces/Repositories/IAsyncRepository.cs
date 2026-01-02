using RestaurantManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Domain.Interfaces.Repositories
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        // EF CRUD
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        // Hybrid SP/custom methods (generic signatures; concrete class can implement)
        Task<T?> GetLastRecordAsync();
        Task<bool> CheckIfRecordExists(string name, decimal percentage);
        DataTable GetSalesOrderItemsBySOId(int id);
        Task<IEnumerable<Role>> GetActiveRolesBySpAsync();

        Task<User?> ValidateUserSpAsync(string username, string passwordHash);
        Task<IEnumerable<Page>> GetUserMenuBySpAsync(int userId);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate); // 🔑 new method

    }
}
