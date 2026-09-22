using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    // 22.01.2026 - Vara loendi rea DTO
    [ExcludeFromCodeCoverage]
    public class AssetListItemDto
    {
        public int Id { get; set; }
        public int AssetClassID { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public bool IsRealEstate { get; set; }
        public string AssetClassName { get; set; }
    }
}
