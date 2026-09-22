using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class SaveMonthlyHoldingCommandHandler : IRequestHandler<SaveMonthlyHoldingCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMonthlyHoldingCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMonthlyHoldingCommand request, CancellationToken cancellationToken)
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

            var holding = new MonthlyHolding();
            if (request.Id == 0)
            {
                await _dbContext.MonthlyHoldings.AddAsync(holding);
            }
            else
            {
                holding = await _dbContext.MonthlyHoldings.FindAsync(request.Id);
                if (holding == null)
                {
                    result.AddError("Cannot find monthly holding with ID " + request.Id);
                    return result;
                }
            }

            holding.StateID = request.StateID;
            holding.AssetID = request.AssetID;
            holding.Quantity = request.Quantity;
            holding.Value = request.Value;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
