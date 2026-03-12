using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class DeleteAssetClassCommandHandler : IRequestHandler<DeleteAssetClassCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAssetClassCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAssetClassCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext
                .AssetClasses
                .Where(ac => ac.Id == request.Id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
