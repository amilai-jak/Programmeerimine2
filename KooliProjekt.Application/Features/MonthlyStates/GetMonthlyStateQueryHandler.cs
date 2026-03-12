using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class GetMonthlyStateQueryHandler : IRequestHandler<GetMonthlyStateQuery, OperationResult<object>>
    {
        private readonly IMonthlyStateRepository _monthlyStateRepository;

        public GetMonthlyStateQueryHandler(IMonthlyStateRepository monthlyStateRepository)
        {
            _monthlyStateRepository = monthlyStateRepository;
        }

        public async Task<OperationResult<object>> Handle(GetMonthlyStateQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var state = await _monthlyStateRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = state.Id,
                StateDate = state.StateDate,
                UninvestedCash = state.UninvestedCash,
                Deposits = state.Deposits,
                Withdrawals = state.Withdrawals,
                TotalPortfolioValue = state.TotalPortfolioValue
            };

            return result;
        }
    }
}
