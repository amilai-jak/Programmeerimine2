using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    // 22.01.2026 - list query tagastab OperationResult<PagedResult<AssetListItemDto>>
    // 12.02.2026 - lisatud otsingu parameetrid
    [ExcludeFromCodeCoverage]
    public class ListAssetsQuery : IRequest<OperationResult<PagedResult<AssetListItemDto>>>
    {
        // Suurim lubatud andmete arv andmelehel
        public const int MaxPageSize = 100;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Otsing: vara nimi
        public string Name { get; set; }

        // Otsing: ticker
        public string Ticker { get; set; }

        // Filtreerimine: vara klass
        public int? AssetClassID { get; set; }

        // Filtreerimine: kinnisvara jah/ei
        public bool? IsRealEstate { get; set; }
    }
}
