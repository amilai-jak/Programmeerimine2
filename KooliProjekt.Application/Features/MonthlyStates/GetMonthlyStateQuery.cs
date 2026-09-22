using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    // 16.01.2026 - tagastab MonthlyStateDetailsDto
    [ExcludeFromCodeCoverage]
    public class GetMonthlyStateQuery : IRequest<OperationResult<MonthlyStateDetailsDto>>
    {
        public int Id { get; set; }
    }
}
