using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class GetMonthlyStateQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
    }
}
