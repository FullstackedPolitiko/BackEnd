using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ApiClient
{
    public class FtApiClient
    {
        private static readonly HttpClient client = new HttpClient();
        private string baseUrl = "https://oda.ft.dk/api/Sag";

        public async Task<Sag?> GetSagAsync(int sagid)
        {
            string url = $"{baseUrl}?$filter=id eq {sagid}&$format=json";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"HTTP {(int)response.StatusCode} ({response.StatusCode})\n" +
                    $"URL: {url}\n" +
                    $"Body: {errorBody}"
                );
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var odata = JsonSerializer.Deserialize<ODataResult<Sag>>(json, options);
            return odata?.Value.Length > 0 ? odata.Value[0] : null;
        }

        public async Task<Sagstrin[]> GetSagstrinForSagAsync(int sagid)
        {
            string url =
                $"{baseUrl}({sagid})/Sagstrin?$expand=Sagstrinstype&$format=json";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var odata = JsonSerializer.Deserialize<ODataResult<Sagstrin>>(json, options);
            return odata?.Value ?? Array.Empty<Sagstrin>();
        }
    }

    public class ODataResult<T>
    {
        public T[] Value { get; set; } = Array.Empty<T>();
    }

    public class Sag
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        // Add properties as you need them
    }

    public class Sagstrin
    {
        public int Id { get; set; }
        // public Sagstrinstype Sagstrinstype { get; set; }
        // etc...
    }
}