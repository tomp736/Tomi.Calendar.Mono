using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.DataTable;
using Tomi.Calendar.Proto.CodeFirst;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class RestDataTableServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RestDataTableServiceClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async ValueTask<RestDataTableResult> GetDataTable(RestDataTableRequest restDataTableRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/api/datatable", restDataTableRequest);
            return JsonSerializer.Deserialize<RestDataTableResult>(await response.Content.ReadAsStringAsync());
        }
    }
}
