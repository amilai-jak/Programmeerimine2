using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Assets
{
    public class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAssetCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .Assets
                .Where(a => a.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
