using System.Threading.Tasks;

namespace KooliProjekt.WindowsForms.Api
{
    // 19.03.2026 - API kliendi interface
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Asset>>> List(int page, int pageSize);
        Task<OperationResult> Save(Asset asset);
        Task<OperationResult> Delete(int id);
    }
}
