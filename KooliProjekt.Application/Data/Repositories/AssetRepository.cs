using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        public AssetRepository(ApplicationDbContext dbContext) : 
            base(dbContext)
        {
        }

        public override async Task<Asset> GetByIdAsync(int id)
        {
            return await DbContext
                .Assets
                .Include(a => a.AssetClass)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
