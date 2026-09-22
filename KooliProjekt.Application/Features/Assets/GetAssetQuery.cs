using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    // 16.01.2026 - tagastab AssetDetailsDto
    [ExcludeFromCodeCoverage]
    public class GetAssetQuery : IRequest<OperationResult<AssetDetailsDto>>
    {
        public int Id { get; set; }
    }
}
