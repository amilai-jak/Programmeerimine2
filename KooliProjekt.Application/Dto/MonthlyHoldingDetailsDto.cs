using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    // 16.01.2026 - Kuu hoiu detailide DTO
    [ExcludeFromCodeCoverage]
    public class MonthlyHoldingDetailsDto
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
