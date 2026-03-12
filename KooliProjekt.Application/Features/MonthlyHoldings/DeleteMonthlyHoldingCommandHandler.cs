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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteMonthlyHoldingCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .MonthlyHoldings
                .Where(mh => mh.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
