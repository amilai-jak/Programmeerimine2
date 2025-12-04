using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class List
    {
        public class Query : IRequest<PagedResult<Result>>
        {
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        public class Result
        {
            public int AssetClassID { get; set; }
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedResult<Result>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PagedResult<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.AssetClasses.AsNoTracking();

                var count = await query.CountAsync(cancellationToken);
                var items = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new Result
                    {
                        AssetClassID = p.AssetClassID,
                        Name = p.Name
                    })
                    .ToListAsync(cancellationToken);

                return new PagedResult<Result>
                {
                    Results = items,
                    CurrentPage = request.Page,
                    PageCount = (int)Math.Ceiling(count / (double)request.PageSize),
                    RowCount = count,
                    PageSize = request.PageSize
                };
            }
        }
    }
}
