using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

            // Use System.Linq.Dynamic.Core for dynamic sorting
            return query.OrderBy($"{sortBy} {(isAscending ? "ascending" : "descending")}");
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNr, int pageSize)
        {
            return query.Skip((pageNr - 1) * pageSize).Take(pageSize);
        }
    }

}
