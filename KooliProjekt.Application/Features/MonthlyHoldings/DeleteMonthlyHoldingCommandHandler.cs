using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    public class DeleteMonthlyHoldingCommandHandler : IRequestHandler<DeleteMonthlyHoldingCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteMonthlyHoldingCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteMonthlyHoldingCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            // 23.01.2026 - ExecuteDeleteAsync asemel otsime kirje ules ja kustutame selle
            // (InMemory andmebaas ei toeta ExecuteDeleteAsync meetodit)
            var holding = await _dbContext
                .MonthlyHoldings
                .FirstOrDefaultAsync(holding => holding.Id == request.Id);

            if (holding == null)
            {
                return result;
            }

            _dbContext.MonthlyHoldings.Remove(holding);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
