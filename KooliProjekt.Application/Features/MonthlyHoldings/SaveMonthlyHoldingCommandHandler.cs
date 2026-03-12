using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class SaveMonthlyHoldingCommandHandler : IRequestHandler<SaveMonthlyHoldingCommand, OperationResult>
    {
        private readonly IMonthlyHoldingRepository _monthlyHoldingRepository;

        public SaveMonthlyHoldingCommandHandler(IMonthlyHoldingRepository monthlyHoldingRepository)
        {
            _monthlyHoldingRepository = monthlyHoldingRepository;
        }

        public async Task<OperationResult> Handle(SaveMonthlyHoldingCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var holding = new MonthlyHolding();
            if (request.Id != 0)
            {
                holding = await _monthlyHoldingRepository.GetByIdAsync(request.Id);
            }

            holding.StateID = request.StateID;
            holding.AssetID = request.AssetID;
            holding.Quantity = request.Quantity;
            holding.Value = request.Value;

            await _monthlyHoldingRepository.SaveAsync(holding);

            return result;
        }
    }
}
