using System;
using System.Linq;
using System.Linq.Expressions;
using TRNMNT.Core.Enum;

namespace TRNMNT.Core.Helpers
{
    public static class Extensions
    {
        public static IOrderedQueryable<T> OrderByDirection<T, TKey>(this IQueryable<T> query, SortDirectionEnum sortDirection, Expression<Func<T, TKey>> expression)
        {
            return sortDirection == SortDirectionEnum.Asc ? query.OrderBy(expression) : query.OrderByDescending(expression);
        }
    }
}