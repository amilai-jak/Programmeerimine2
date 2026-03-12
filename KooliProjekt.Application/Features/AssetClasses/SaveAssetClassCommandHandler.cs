using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class SaveAssetClassCommandHandler : IRequestHandler<SaveAssetClassCommand, OperationResult>
    {
        private readonly IAssetClassRepository _assetClassRepository;

        public SaveAssetClassCommandHandler(IAssetClassRepository assetClassRepository)
        {
            _assetClassRepository = assetClassRepository;
        }

        public async Task<OperationResult> Handle(SaveAssetClassCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var assetClass = new AssetClass();
            if (request.Id != 0)
            {
                assetClass = await _assetClassRepository.GetByIdAsync(request.Id);
            }

            assetClass.Name = request.Name;

            await _assetClassRepository.SaveAsync(assetClass);

            return result;
        }
    }
}
