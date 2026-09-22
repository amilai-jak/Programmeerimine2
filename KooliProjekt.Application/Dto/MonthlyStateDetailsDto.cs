using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Dto
{
    // 16.01.2026 - Kuu oleku detailide DTO
    [ExcludeFromCodeCoverage]
    public class MonthlyStateDetailsDto
    {
        public int Id { get; set; }
        public DateTime StateDate { get; set; }
        public decimal UninvestedCash { get; set; }
        public decimal Deposits { get; set; }
        public decimal Withdrawals { get; set; }
        public decimal TotalPortfolioValue { get; set; }
    }
}
