using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class SaveMonthlyStateCommandHandler : IRequestHandler<SaveMonthlyStateCommand, OperationResult>
    {
        private readonly IMonthlyStateRepository _monthlyStateRepository;

        public SaveMonthlyStateCommandHandler(IMonthlyStateRepository monthlyStateRepository)
        {
            _monthlyStateRepository = monthlyStateRepository;
        }

        public async Task<OperationResult> Handle(SaveMonthlyStateCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var state = new MonthlyState();
            if (request.Id != 0)
            {
                state = await _monthlyStateRepository.GetByIdAsync(request.Id);
            }

            state.StateDate = request.StateDate;
            state.UninvestedCash = request.UninvestedCash;
            state.Deposits = request.Deposits;
            state.Withdrawals = request.Withdrawals;
            state.TotalPortfolioValue = request.TotalPortfolioValue;

            await _monthlyStateRepository.SaveAsync(state);

            return result;
        }
    }
}
