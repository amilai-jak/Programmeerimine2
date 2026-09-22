using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    // 16.01.2026 - tagastab AssetClassDetailsDto
    [ExcludeFromCodeCoverage]
    public class GetAssetClassQuery : IRequest<OperationResult<AssetClassDetailsDto>>
    {
        public int Id { get; set; }
    }
}
