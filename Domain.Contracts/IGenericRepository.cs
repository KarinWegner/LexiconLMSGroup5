using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> PatchAsync(int id, JsonPatchDocument<T> patchDoc);
        Task<bool> SaveChangesAsync();

        // Method to get IQueryable to perform further query operations (like Include, Where)
        IQueryable<T> Query();
    }
}
