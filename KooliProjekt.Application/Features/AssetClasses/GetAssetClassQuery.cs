using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class GetAssetClassQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
