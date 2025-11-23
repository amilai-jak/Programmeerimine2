using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                var items = await query
                    .Select(p => new Result
                    {
                        AssetClassID = p.AssetClassID,
                        Name = p.Name
                    })
                    .ToListAsync(cancellationToken);

                return new PagedResult<Result>
                {
                    Results = items,
                    RowCount = await query.CountAsync(cancellationToken)
                };
            }
        }
    }
}
