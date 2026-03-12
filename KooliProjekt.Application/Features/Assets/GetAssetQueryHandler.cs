using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    public class GetAssetQueryHandler : IRequestHandler<GetAssetQuery, OperationResult<object>>
    {
        private readonly IAssetRepository _assetRepository;

        public GetAssetQueryHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAssetQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var asset = await _assetRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = asset.Id,
                AssetClassID = asset.AssetClassID,
                Name = asset.Name,
                Ticker = asset.Ticker,
                IsRealEstate = asset.IsRealEstate,
                AssetClassName = asset.AssetClass?.Name
            };

            return result;
        }
    }
}
