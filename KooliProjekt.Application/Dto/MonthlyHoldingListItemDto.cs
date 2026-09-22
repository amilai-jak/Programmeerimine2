using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    // 22.01.2026 - Kuu hoiu loendi rea DTO
    [ExcludeFromCodeCoverage]
    public class MonthlyHoldingListItemDto
    {
        public int Id { get; set; }
        public int StateID { get; set; }
        public int AssetID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }
        public string AssetName { get; set; }
        public DateTime StateDate { get; set; }
    }
}
