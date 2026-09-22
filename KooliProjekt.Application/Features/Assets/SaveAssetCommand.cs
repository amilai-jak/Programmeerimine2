using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    // 15.11.2025
    // Vara salvestamise kask
    [ExcludeFromCodeCoverage]
    public class SaveAssetCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int AssetClassID { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public bool IsRealEstate { get; set; }
    }
}
