using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    // 15.11.2025
    // Kuu oleku kustutamise kask
    [ExcludeFromCodeCoverage]
    public class DeleteMonthlyStateCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
