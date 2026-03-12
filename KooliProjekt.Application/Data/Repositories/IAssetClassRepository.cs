using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IAssetClassRepository
    {
        Task<AssetClass> GetByIdAsync(int id);
        Task SaveAsync(AssetClass entity);
        Task DeleteAsync(AssetClass entity);
    }
}
