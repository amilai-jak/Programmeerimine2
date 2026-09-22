using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Assets
{
    public class SaveAssetCommandHandler : IRequestHandler<SaveAssetCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAssetCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAssetCommand request, CancellationToken cancellationToken)
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

            var asset = new Asset();
            if (request.Id == 0)
            {
                await _dbContext.Assets.AddAsync(asset);
            }
            else
            {
                asset = await _dbContext.Assets.FindAsync(request.Id);
                if (asset == null)
                {
                    result.AddError("Cannot find asset with ID " + request.Id);
                    return result;
                }
            }

            asset.AssetClassID = request.AssetClassID;
            asset.Name = request.Name;
            asset.Ticker = request.Ticker;
            asset.IsRealEstate = request.IsRealEstate;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
