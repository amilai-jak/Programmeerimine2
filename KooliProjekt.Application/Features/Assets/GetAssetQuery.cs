using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    public class GetAssetQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
