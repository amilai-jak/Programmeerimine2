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

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class ListMonthlyHoldingsQueryHandler : IRequestHandler<ListMonthlyHoldingsQuery, OperationResult<PagedResult<MonthlyHoldingListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListMonthlyHoldingsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<MonthlyHoldingListItemDto>>> Handle(ListMonthlyHoldingsQuery request, CancellationToken cancellationToken)
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

            if (request.PageSize > ListMonthlyHoldingsQuery.MaxPageSize)
            {
                throw new ArgumentException("PageSize cannot be greater than " + ListMonthlyHoldingsQuery.MaxPageSize, nameof(request.PageSize));
            }

            var result = new OperationResult<PagedResult<MonthlyHoldingListItemDto>>();

            var query = _dbContext
                .MonthlyHoldings
                .Include(holding => holding.Asset)
                .Include(holding => holding.MonthlyState)
                .AsNoTracking();

            // 12.02.2026 - otsingu ja filtreerimise tingimused
            if (!string.IsNullOrEmpty(request.AssetName))
            {
                query = query.Where(holding => holding.Asset.Name.Contains(request.AssetName));
            }

            if (request.AssetID.HasValue)
            {
                query = query.Where(holding => holding.AssetID == request.AssetID.Value);
            }

            if (request.StateID.HasValue)
            {
                query = query.Where(holding => holding.StateID == request.StateID.Value);
            }

            result.Value = await query
                .OrderBy(holding => holding.Asset.Name)
                .Select(holding => new MonthlyHoldingListItemDto
                {
                    Id = holding.Id,
                    StateID = holding.StateID,
                    AssetID = holding.AssetID,
                    Quantity = holding.Quantity,
                    Value = holding.Value,
                    AssetName = holding.Asset.Name,
                    StateDate = holding.MonthlyState.StateDate
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
