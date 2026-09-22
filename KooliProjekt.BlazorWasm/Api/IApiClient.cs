namespace KooliProjekt.BlazorWasm
{
    public interface IApiClient
    {
        Task<OperationResult<Asset>> Get(int id);
        Task<OperationResult<PagedResult<Asset>>> List(int page, int pageSize);
        Task<OperationResult> Save(Asset asset);
        Task<OperationResult> Delete(int id);
    }
}
