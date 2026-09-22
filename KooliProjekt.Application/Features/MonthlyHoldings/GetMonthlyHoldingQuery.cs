using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    // 16.01.2026 - tagastab MonthlyHoldingDetailsDto
    [ExcludeFromCodeCoverage]
    public class GetMonthlyHoldingQuery : IRequest<OperationResult<MonthlyHoldingDetailsDto>>
    {
        public int Id { get; set; }
    }
}
