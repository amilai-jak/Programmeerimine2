using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAssetClassCommand request, CancellationToken cancellationToken)
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
            var assetClass = await _dbContext
                .AssetClasses
                .FirstOrDefaultAsync(assetClass => assetClass.Id == request.Id);

            if (assetClass == null)
            {
                return result;
            }

            _dbContext.AssetClasses.Remove(assetClass);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
