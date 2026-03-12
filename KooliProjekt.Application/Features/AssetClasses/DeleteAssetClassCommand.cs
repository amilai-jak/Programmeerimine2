using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class DeleteAssetClassCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
