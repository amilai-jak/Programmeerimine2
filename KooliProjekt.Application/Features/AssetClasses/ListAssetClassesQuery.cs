using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    // 22.01.2026 - list query tagastab OperationResult<PagedResult<AssetClassListItemDto>>
    // 12.02.2026 - lisatud otsingu parameeter
    [ExcludeFromCodeCoverage]
    public class ListAssetClassesQuery : IRequest<OperationResult<PagedResult<AssetClassListItemDto>>>
    {
        // Suurim lubatud andmete arv andmelehel
        public const int MaxPageSize = 100;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Otsing: vara klassi nimi
        public string Name { get; set; }
    }
}
