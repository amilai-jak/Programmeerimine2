using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IAssetRepository
    {
        Task<Asset> GetByIdAsync(int id);
        Task SaveAsync(Asset entity);
        Task DeleteAsync(Asset entity);
    }
}
