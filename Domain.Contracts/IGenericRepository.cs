using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync();

        Task<int> GetTotalCountAsync();

        Task<(IEnumerable<T> Items, int TotalCount)> GetFilteredAndSortedEntitiesAsync(
            Expression<Func<T, bool>>? filter,
            string? sortBy,
            bool isAscending,
            int? pageNr,
            int? pageSize,
            params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> PatchAsync(int id, JsonPatchDocument<T> patchDoc);
        Task<bool> SaveChangesAsync();

        // Method to get IQueryable to perform further query operations (like Include, Where)
        IQueryable<T> Query();
    }
}
