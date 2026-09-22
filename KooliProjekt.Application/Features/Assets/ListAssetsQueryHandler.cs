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

namespace KooliProjekt.Application.Features.Assets
{
    public class ListAssetsQueryHandler : IRequestHandler<ListAssetsQuery, OperationResult<PagedResult<AssetListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAssetsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<AssetListItemDto>>> Handle(ListAssetsQuery request, CancellationToken cancellationToken)
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

            if (request.PageSize > ListAssetsQuery.MaxPageSize)
            {
                throw new ArgumentException("PageSize cannot be greater than " + ListAssetsQuery.MaxPageSize, nameof(request.PageSize));
            }

            var result = new OperationResult<PagedResult<AssetListItemDto>>();

            var query = _dbContext
                .Assets
                .Include(asset => asset.AssetClass)
                .AsNoTracking();

            // 12.02.2026 - otsingu ja filtreerimise tingimused
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(asset => asset.Name.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.Ticker))
            {
                query = query.Where(asset => asset.Ticker.Contains(request.Ticker));
            }

            if (request.AssetClassID.HasValue)
            {
                query = query.Where(asset => asset.AssetClassID == request.AssetClassID.Value);
            }

            if (request.IsRealEstate.HasValue)
            {
                query = query.Where(asset => asset.IsRealEstate == request.IsRealEstate.Value);
            }

            result.Value = await query
                .OrderBy(asset => asset.Name)
                .Select(asset => new AssetListItemDto
                {
                    Id = asset.Id,
                    AssetClassID = asset.AssetClassID,
                    Name = asset.Name,
                    Ticker = asset.Ticker,
                    IsRealEstate = asset.IsRealEstate,
                    AssetClassName = asset.AssetClass.Name
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
