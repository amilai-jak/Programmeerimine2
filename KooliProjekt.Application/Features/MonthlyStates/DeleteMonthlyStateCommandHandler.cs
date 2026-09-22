using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    public class DeleteMonthlyStateCommandHandler : IRequestHandler<DeleteMonthlyStateCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteMonthlyStateCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteMonthlyStateCommand request, CancellationToken cancellationToken)
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
            var state = await _dbContext
                .MonthlyStates
                .FirstOrDefaultAsync(state => state.Id == request.Id);

            if (state == null)
            {
                return result;
            }

            _dbContext.MonthlyStates.Remove(state);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
