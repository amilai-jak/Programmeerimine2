using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    // 15.11.2025
    // Kuu hoiu kustutamise kask
    [ExcludeFromCodeCoverage]
    public class DeleteMonthlyHoldingCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
