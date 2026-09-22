using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AssetClasses
{
    public class SaveAssetClassCommandHandler : IRequestHandler<SaveAssetClassCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAssetClassCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAssetClassCommand request, CancellationToken cancellationToken)
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

            var assetClass = new AssetClass();
            if (request.Id == 0)
            {
                await _dbContext.AssetClasses.AddAsync(assetClass);
            }
            else
            {
                assetClass = await _dbContext.AssetClasses.FindAsync(request.Id);
                if (assetClass == null)
                {
                    result.AddError("Cannot find asset class with ID " + request.Id);
                    return result;
                }
            }

            assetClass.Name = request.Name;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
