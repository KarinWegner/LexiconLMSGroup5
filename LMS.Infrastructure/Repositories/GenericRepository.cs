using Domain.Contracts;
using LMS.Infrastructure.Data;
using LMS.Shared.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



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

            var entityTypeName = typeof(T).Name; 
            var primaryKeyName = $"{entityTypeName}Id";

            return await query.FirstOrDefaultAsync(entity => EF.Property<int>(entity, primaryKeyName).Equals(id));
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetFilteredAndSortedEntitiesAsync(
         Expression<Func<T, bool>>? filter,
         string? sortBy,
         bool isAscending,
         int? pageNr,
         int? pageSize,
         params Expression<Func<T, object>>[] includes)
        {

            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

             query = query.ApplyFiltering(filter)
                .ApplySorting(sortBy, isAscending);

            // Get the total count before pagination
            var totalCount = await query.CountAsync();

            if (pageNr.HasValue && pageSize.HasValue)
            {
                query = query.ApplyPagination(pageNr.Value, pageSize.Value);
            }

            var items = await query.ToListAsync();
            return (Items: items, TotalCount: totalCount);
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            var all = await _dbSet.ToListAsync();
            return all.Count;
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
