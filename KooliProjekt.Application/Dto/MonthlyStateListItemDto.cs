using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    // 22.01.2026 - Kuu oleku loendi rea DTO
    [ExcludeFromCodeCoverage]
    public class MonthlyStateListItemDto
    {
        public int Id { get; set; }
        public DateTime StateDate { get; set; }
        public decimal UninvestedCash { get; set; }
        public decimal Deposits { get; set; }
        public decimal Withdrawals { get; set; }
        public decimal TotalPortfolioValue { get; set; }
    }
}
