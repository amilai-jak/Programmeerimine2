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

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class List
    {
        public class Query : IRequest<PagedResult<Result>>
        {
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

                var items = await query
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
                    RowCount = await query.CountAsync(cancellationToken)
                };
            }
        }
    }
}
