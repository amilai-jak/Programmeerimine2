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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteMonthlyStateCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .MonthlyStates
                .Where(ms => ms.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
