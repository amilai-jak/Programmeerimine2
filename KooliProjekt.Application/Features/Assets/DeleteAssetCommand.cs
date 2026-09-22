using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    // 15.11.2025
    // Vara kustutamise kask
    [ExcludeFromCodeCoverage]
    public class DeleteAssetCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
