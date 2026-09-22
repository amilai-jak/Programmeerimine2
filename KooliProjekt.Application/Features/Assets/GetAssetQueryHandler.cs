using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Assets
{
    public class GetAssetQueryHandler : IRequestHandler<GetAssetQuery, OperationResult<AssetDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAssetQueryHandler(ApplicationDbContext dbContext)
        {
            // 16.01.2026 - lisatud nullkontroll
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<AssetDetailsDto>> Handle(GetAssetQuery request, CancellationToken cancellationToken)
        {
            // 22.01.2026 - null request ei tohi andmebaasi poole jouda
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<AssetDetailsDto>();

            // 22.01.2026 - vigase ID-ga ei tehta andmebaasi paringut
            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Assets
                .Include(asset => asset.AssetClass)
                .Where(asset => asset.Id == request.Id)
                .Select(asset => new AssetDetailsDto
                {
                    Id = asset.Id,
                    AssetClassID = asset.AssetClassID,
                    Name = asset.Name,
                    Ticker = asset.Ticker,
                    IsRealEstate = asset.IsRealEstate,
                    AssetClassName = asset.AssetClass.Name
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
