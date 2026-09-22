using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KooliProjekt.WindowsForms.Api
{
    // 19.03.2026 - API klient, mis suhtleb KooliProjekt.WebAPI-ga
    // 20.03.2026 - API-st tagastatavad vead (valideerimine jne) jouavad OperationResult sisse
    public class ApiClient : IApiClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public ApiClient()
        {
            _baseUrl = "http://localhost:5086/api/Assets/";
            _client = new HttpClient();
        }

        public async Task<OperationResult<PagedResult<Asset>>> List(int page, int pageSize)
        {
            var url = _baseUrl + "List?page=" + page + "&pageSize=" + pageSize;

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult<PagedResult<Asset>>>(body);
            return result;
        }

        public async Task<OperationResult> Save(Asset asset)
        {
            var url = _baseUrl + "Save";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(asset)
            };
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }

        public async Task<OperationResult> Delete(int id)
        {
            var url = _baseUrl + "Delete";

            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = id })
            };
            using var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<OperationResult>(body);
            return result;
        }
    }
}
