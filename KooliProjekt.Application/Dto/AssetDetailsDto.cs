using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    // 16.01.2026 - Vara detailide DTO
    [ExcludeFromCodeCoverage]
    public class AssetDetailsDto
    {
        public int Id { get; set; }
        public int AssetClassID { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public bool IsRealEstate { get; set; }
        public string AssetClassName { get; set; }
    }
}
