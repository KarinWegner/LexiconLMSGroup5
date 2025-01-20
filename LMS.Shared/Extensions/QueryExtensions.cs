using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace LMS.Shared.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, Expression<Func<T, bool>>? filter)
        {
            return filter != null ? query.Where(filter) : query;
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, bool isAscending)
        {
            if (string.IsNullOrEmpty(sortBy)) return query;

            // Using System.Linq.Dynamic.Core for dynamic sorting
            var sortDirection = isAscending ? "ascending" : "descending";
            return query.OrderBy($"{sortBy} {sortDirection}");
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNr, int pageSize)
        {
            return query.Skip((pageNr - 1) * pageSize).Take(pageSize);
        }
    }
}
