using System;
using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    // 22.01.2026 - list query tagastab OperationResult<PagedResult<MonthlyStateListItemDto>>
    // 12.02.2026 - lisatud otsingu parameetrid
    [ExcludeFromCodeCoverage]
    public class ListMonthlyStatesQuery : IRequest<OperationResult<PagedResult<MonthlyStateListItemDto>>>
    {
        // Suurim lubatud andmete arv andmelehel
        public const int MaxPageSize = 100;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Otsing: kuupaev alates
        public DateTime? StateDateFrom { get; set; }

        // Otsing: kuupaev kuni
        public DateTime? StateDateTo { get; set; }

        // Otsing: vaartuse alumine piir
        public decimal? MinTotalPortfolioValue { get; set; }
    }
}
