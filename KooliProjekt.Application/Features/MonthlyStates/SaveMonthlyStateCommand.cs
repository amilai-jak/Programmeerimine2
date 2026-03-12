using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class SaveMonthlyStateCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public DateTime StateDate { get; set; }
        public decimal UninvestedCash { get; set; }
        public decimal Deposits { get; set; }
        public decimal Withdrawals { get; set; }
        public decimal TotalPortfolioValue { get; set; }
    }
}
