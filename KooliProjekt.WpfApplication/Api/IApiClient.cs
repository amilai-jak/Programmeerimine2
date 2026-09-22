namespace KooliProjekt.WpfApplication.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Asset>>> List(int page, int pageSize);
        Task<OperationResult> Save(Asset asset);
        Task<OperationResult> Delete(int id);
    }
}
