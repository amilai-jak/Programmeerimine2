using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    // 15.11.2025
    // Kuu hoiu salvestamise kask
    [ExcludeFromCodeCoverage]
    public class SaveMonthlyHoldingCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int StateID { get; set; }
        public int AssetID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }
    }
}
