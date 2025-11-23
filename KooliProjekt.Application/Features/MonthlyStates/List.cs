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

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class List
    {
        public class Query : IRequest<PagedResult<Result>>
        {
        }

        public class Result
        {
            public int StateID { get; set; }
            public DateTime StateDate { get; set; }
            public decimal UninvestedCash { get; set; }
            public decimal Deposits { get; set; }
            public decimal Withdrawals { get; set; }
            public decimal TotalPortfolioValue { get; set; }
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
                var query = _context.MonthlyStates.AsNoTracking();

                var items = await query
                    .Select(p => new Result
                    {
                        StateID = p.StateID,
                        StateDate = p.StateDate,
                        UninvestedCash = p.UninvestedCash,
                        Deposits = p.Deposits,
                        Withdrawals = p.Withdrawals,
                        TotalPortfolioValue = p.TotalPortfolioValue
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
