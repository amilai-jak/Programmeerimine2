using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MonthlyHoldings
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
            public int HoldingID { get; set; }
            public int StateID { get; set; }
            public int AssetID { get; set; }
            public decimal Quantity { get; set; }
            public decimal Value { get; set; }
            public string AssetName { get; set; }
            public DateTime StateDate { get; set; }
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
                var query = _context.MonthlyHoldings
                                    .Include(mh => mh.Asset)
                                    .Include(mh => mh.MonthlyState)
                                    .AsNoTracking();

                var count = await query.CountAsync(cancellationToken);
                var items = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new Result
                    {
                        HoldingID = p.HoldingID,
                        StateID = p.StateID,
                        AssetID = p.AssetID,
                        Quantity = p.Quantity,
                        Value = p.Value,
                        AssetName = p.Asset.Name,
                        StateDate = p.MonthlyState.StateDate
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
