using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class GetMonthlyStateQueryHandler : IRequestHandler<GetMonthlyStateQuery, OperationResult<MonthlyStateDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMonthlyStateQueryHandler(ApplicationDbContext dbContext)
        {
            // 16.01.2026 - lisatud nullkontroll
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult<MonthlyStateDetailsDto>> Handle(GetMonthlyStateQuery request, CancellationToken cancellationToken)
        {
            // 22.01.2026 - null request ei tohi andmebaasi poole jouda
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<MonthlyStateDetailsDto>();

            // 22.01.2026 - vigase ID-ga ei tehta andmebaasi paringut
            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .MonthlyStates
                .Where(state => state.Id == request.Id)
                .Select(state => new MonthlyStateDetailsDto
                {
                    Id = state.Id,
                    StateDate = state.StateDate,
                    UninvestedCash = state.UninvestedCash,
                    Deposits = state.Deposits,
                    Withdrawals = state.Withdrawals,
                    TotalPortfolioValue = state.TotalPortfolioValue
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
