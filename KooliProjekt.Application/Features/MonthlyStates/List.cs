using System;
using System.Linq;
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
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
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

                var count = await query.CountAsync(cancellationToken);
                var items = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
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
                    CurrentPage = request.Page,
                    PageCount = (int)Math.Ceiling(count / (double)request.PageSize),
                    RowCount = count,
                    PageSize = request.PageSize
                };
            }
        }
    }
}
