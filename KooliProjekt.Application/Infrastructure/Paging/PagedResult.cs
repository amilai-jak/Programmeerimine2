using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace KooliProjekt.Application.Infrastructure.Paging
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}