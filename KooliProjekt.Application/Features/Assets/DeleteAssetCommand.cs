using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    public class DeleteAssetCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
