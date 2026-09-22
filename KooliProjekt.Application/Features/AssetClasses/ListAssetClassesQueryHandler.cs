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

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class ListAssetClassesQueryHandler : IRequestHandler<ListAssetClassesQuery, OperationResult<PagedResult<AssetClassListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAssetClassesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<AssetClassListItemDto>>> Handle(ListAssetClassesQuery request, CancellationToken cancellationToken)
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

            if (request.PageSize > ListAssetClassesQuery.MaxPageSize)
            {
                throw new ArgumentException("PageSize cannot be greater than " + ListAssetClassesQuery.MaxPageSize, nameof(request.PageSize));
            }

            var result = new OperationResult<PagedResult<AssetClassListItemDto>>();

            var query = _dbContext
                .AssetClasses
                .AsNoTracking();

            // 12.02.2026 - otsingu tingimus
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(assetClass => assetClass.Name.Contains(request.Name));
            }

            result.Value = await query
                .OrderBy(assetClass => assetClass.Name)
                .Select(assetClass => new AssetClassListItemDto
                {
                    Id = assetClass.Id,
                    Name = assetClass.Name
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
