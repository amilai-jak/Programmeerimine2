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

namespace KooliProjekt.Application.Features.Assets
{
    public class List
    {
        public class Query : IRequest<PagedResult<Result>>
        {
        }

        public class Result
        {
            public int AssetID { get; set; }
            public int AssetClassID { get; set; }
            public string Name { get; set; }
            public string Ticker { get; set; }
            public bool IsRealEstate { get; set; }
            public string AssetClassName { get; set; }
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
                var query = _context.Assets
                                    .Include(a => a.AssetClass)
                                    .AsNoTracking();

                var items = await query
                    .Select(p => new Result
                    {
                        AssetID = p.AssetID,
                        AssetClassID = p.AssetClassID,
                        Name = p.Name,
                        Ticker = p.Ticker,
                        IsRealEstate = p.IsRealEstate,
                        AssetClassName = p.AssetClass.Name
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
