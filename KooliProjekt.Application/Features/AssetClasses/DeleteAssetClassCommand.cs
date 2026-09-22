using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    // 15.11.2025
    // Vara klassi kustutamise kask
    [ExcludeFromCodeCoverage]
    public class DeleteAssetClassCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
