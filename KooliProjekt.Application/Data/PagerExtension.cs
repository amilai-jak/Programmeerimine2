using System;
using System.Collections.Generic;
using System.Linq;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data
{
    public static class PagerExtension
    {
        public static PagedResult<T> GetPaged<T>(this IEnumerable<T> query, int page, int pageSize) where T : class
        {
            var count = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<T>()
            {
                Results = items,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling(count / (double)pageSize),
                RowCount = count,
                PageSize = pageSize
            };
        }
    }
}
