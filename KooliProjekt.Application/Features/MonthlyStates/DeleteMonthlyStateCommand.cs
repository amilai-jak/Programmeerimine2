using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class DeleteMonthlyStateCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
    }
}
