using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    public class SaveAssetCommandHandler : IRequestHandler<SaveAssetCommand, OperationResult>
    {
        private readonly IAssetRepository _assetRepository;

        public SaveAssetCommandHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<OperationResult> Handle(SaveAssetCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var asset = new Asset();
            if (request.Id != 0)
            {
                asset = await _assetRepository.GetByIdAsync(request.Id);
            }

            asset.AssetClassID = request.AssetClassID;
            asset.Name = request.Name;
            asset.Ticker = request.Ticker;
            asset.IsRealEstate = request.IsRealEstate;

            await _assetRepository.SaveAsync(asset);

            return result;
        }
    }
}
