using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class DeleteMonthlyHoldingCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
