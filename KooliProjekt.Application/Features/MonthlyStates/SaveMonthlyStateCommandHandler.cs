using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class SaveMonthlyStateCommandHandler : IRequestHandler<SaveMonthlyStateCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMonthlyStateCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMonthlyStateCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            // 05.02.2026 - negatiivse ID-ga ei minda andmebaasi poole
            if (request.Id < 0)
            {
                result.AddError("Request ID cannot be negative");
                return result;
            }

            var state = new MonthlyState();
            if (request.Id == 0)
            {
                await _dbContext.MonthlyStates.AddAsync(state);
            }
            else
            {
                state = await _dbContext.MonthlyStates.FindAsync(request.Id);
                if (state == null)
                {
                    result.AddError("Cannot find monthly state with ID " + request.Id);
                    return result;
                }
            }

            state.StateDate = request.StateDate;
            state.UninvestedCash = request.UninvestedCash;
            state.Deposits = request.Deposits;
            state.Withdrawals = request.Withdrawals;
            state.TotalPortfolioValue = request.TotalPortfolioValue;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
