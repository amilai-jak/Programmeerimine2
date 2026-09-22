using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class ListMonthlyStatesQueryHandler : IRequestHandler<ListMonthlyStatesQuery, OperationResult<PagedResult<MonthlyStateListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListMonthlyStatesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<MonthlyStateListItemDto>>> Handle(ListMonthlyStatesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // 22.01.2026 - vigaste leheparameetritega ei tehta andmebaasi paringut
            if (request.Page <= 0)
            {
                throw new ArgumentException("Page must be greater than zero", nameof(request.Page));
            }

            if (request.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be greater than zero", nameof(request.PageSize));
            }

            if (request.PageSize > ListMonthlyStatesQuery.MaxPageSize)
            {
                throw new ArgumentException("PageSize cannot be greater than " + ListMonthlyStatesQuery.MaxPageSize, nameof(request.PageSize));
            }

            var result = new OperationResult<PagedResult<MonthlyStateListItemDto>>();

            var query = _dbContext
                .MonthlyStates
                .AsNoTracking();

            // 12.02.2026 - otsingu ja filtreerimise tingimused
            if (request.StateDateFrom.HasValue)
            {
                query = query.Where(state => state.StateDate >= request.StateDateFrom.Value);
            }

            if (request.StateDateTo.HasValue)
            {
                query = query.Where(state => state.StateDate <= request.StateDateTo.Value);
            }

            if (request.MinTotalPortfolioValue.HasValue)
            {
                query = query.Where(state => state.TotalPortfolioValue >= request.MinTotalPortfolioValue.Value);
            }

            result.Value = await query
                .OrderByDescending(state => state.StateDate)
                .Select(state => new MonthlyStateListItemDto
                {
                    Id = state.Id,
                    StateDate = state.StateDate,
                    UninvestedCash = state.UninvestedCash,
                    Deposits = state.Deposits,
                    Withdrawals = state.Withdrawals,
                    TotalPortfolioValue = state.TotalPortfolioValue
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
