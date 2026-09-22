using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class GetMonthlyHoldingQueryHandler : IRequestHandler<GetMonthlyHoldingQuery, OperationResult<MonthlyHoldingDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMonthlyHoldingQueryHandler(ApplicationDbContext dbContext)
        {
            // 16.01.2026 - lisatud nullkontroll
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<MonthlyHoldingDetailsDto>> Handle(GetMonthlyHoldingQuery request, CancellationToken cancellationToken)
        {
            // 22.01.2026 - null request ei tohi andmebaasi poole jouda
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<MonthlyHoldingDetailsDto>();

            // 22.01.2026 - vigase ID-ga ei tehta andmebaasi paringut
            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .MonthlyHoldings
                .Include(holding => holding.Asset)
                .Include(holding => holding.MonthlyState)
                .Where(holding => holding.Id == request.Id)
                .Select(holding => new MonthlyHoldingDetailsDto
                {
                    Id = holding.Id,
                    StateID = holding.StateID,
                    AssetID = holding.AssetID,
                    Quantity = holding.Quantity,
                    Value = holding.Value,
                    AssetName = holding.Asset.Name,
                    StateDate = holding.MonthlyState.StateDate
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
