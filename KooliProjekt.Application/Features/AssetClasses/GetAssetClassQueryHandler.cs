using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class GetAssetClassQueryHandler : IRequestHandler<GetAssetClassQuery, OperationResult<AssetClassDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAssetClassQueryHandler(ApplicationDbContext dbContext)
        {
            // 16.01.2026 - lisatud nullkontroll
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<AssetClassDetailsDto>> Handle(GetAssetClassQuery request, CancellationToken cancellationToken)
        {
            // 22.01.2026 - null request ei tohi andmebaasi poole jouda
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<AssetClassDetailsDto>();

            // 22.01.2026 - vigase ID-ga ei tehta andmebaasi paringut
            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .AssetClasses
                .Where(assetClass => assetClass.Id == request.Id)
                .Select(assetClass => new AssetClassDetailsDto
                {
                    Id = assetClass.Id,
                    Name = assetClass.Name
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
