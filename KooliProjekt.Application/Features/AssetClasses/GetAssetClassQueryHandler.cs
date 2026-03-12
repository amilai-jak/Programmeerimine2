using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class GetAssetClassQueryHandler : IRequestHandler<GetAssetClassQuery, OperationResult<object>>
    {
        private readonly IAssetClassRepository _assetClassRepository;

        public GetAssetClassQueryHandler(IAssetClassRepository assetClassRepository)
        {
            _assetClassRepository = assetClassRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAssetClassQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var assetClass = await _assetClassRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = assetClass.Id,
                Name = assetClass.Name
            };

            return result;
        }
    }
}
