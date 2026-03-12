using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class GetMonthlyHoldingQueryHandler : IRequestHandler<GetMonthlyHoldingQuery, OperationResult<object>>
    {
        private readonly IMonthlyHoldingRepository _monthlyHoldingRepository;

        public GetMonthlyHoldingQueryHandler(IMonthlyHoldingRepository monthlyHoldingRepository)
        {
            _monthlyHoldingRepository = monthlyHoldingRepository;
        }

        public async Task<OperationResult<object>> Handle(GetMonthlyHoldingQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var holding = await _monthlyHoldingRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = holding.Id,
                StateID = holding.StateID,
                AssetID = holding.AssetID,
                Quantity = holding.Quantity,
                Value = holding.Value,
                AssetName = holding.Asset?.Name,
                StateDate = holding.MonthlyState?.StateDate
            };

            return result;
        }
    }
}
