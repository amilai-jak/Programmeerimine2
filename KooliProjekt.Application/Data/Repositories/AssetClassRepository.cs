namespace KooliProjekt.Application.Data.Repositories
{
    public class AssetClassRepository : BaseRepository<AssetClass>, IAssetClassRepository
    {
        public AssetClassRepository(ApplicationDbContext dbContext) : 
            base(dbContext)
        {
        }
    }
}
