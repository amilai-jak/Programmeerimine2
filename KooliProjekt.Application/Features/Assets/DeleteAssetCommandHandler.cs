using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
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
            var asset = await _dbContext
                .Assets
                .FirstOrDefaultAsync(asset => asset.Id == request.Id);

            if (asset == null)
            {
                return result;
            }

            _dbContext.Assets.Remove(asset);

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
