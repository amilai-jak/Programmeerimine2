using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    // 22.01.2026 - list query tagastab OperationResult<PagedResult<MonthlyHoldingListItemDto>>
    // 12.02.2026 - lisatud otsingu parameetrid
    [ExcludeFromCodeCoverage]
    public class ListMonthlyHoldingsQuery : IRequest<OperationResult<PagedResult<MonthlyHoldingListItemDto>>>
    {
        // Suurim lubatud andmete arv andmelehel
        public const int MaxPageSize = 100;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Otsing: vara nimi
        public string AssetName { get; set; }

        // Filtreerimine: vara
        public int? AssetID { get; set; }

        // Filtreerimine: kuu olek
        public int? StateID { get; set; }
    }
}
