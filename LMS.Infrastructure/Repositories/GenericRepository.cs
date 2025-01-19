using Domain.Contracts;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;


namespace LMS.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LmsContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(LmsContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityTypeName = typeof(T).Name; // Get the entity's type name (e.g., "Course", "Module")
            var primaryKeyName = $"{entityTypeName}Id";

            return await query.FirstOrDefaultAsync(entity => EF.Property<int>(entity, primaryKeyName).Equals(id));
        }


        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await _dbSet.FindAsync(id);
        //}

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFilteredAndSortedAsync(
        Expression<Func<T, bool>>? filter = null,
        string? sortBy = null,
        bool isAscending = true,
        int pageNr = 1,
        int pageSize = 10)
        {
            var query = _dbSet.AsQueryable();

            // Apply filtering
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = query.OrderBy($"{sortBy} {(isAscending ? "ascending" : "descending")}");
            }

            // Apply pagination
            query = query.Skip((pageNr - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }


        public async Task<T> PatchAsync(int id, JsonPatchDocument<T> patchDoc)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return null;

            patchDoc.ApplyTo(entity);

            return entity;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
