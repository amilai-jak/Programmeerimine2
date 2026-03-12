using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class GetMonthlyHoldingQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
