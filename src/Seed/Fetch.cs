using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Shared;

namespace Seed;

public class Fetch
{
    private readonly HttpClient _httpClient;

    public Fetch()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        _httpClient.DefaultRequestHeaders.Add("Client-ID", "i0s32q3oi8z074rvq0ljkaupnbkq98");
        _httpClient.DefaultRequestHeaders.Add("Authorization", "");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    // Fetch total count from the /count endpoint
    public async Task<List<T>> FetchAllDataAsync<T>(string endpoint, string fields)
    {

        var allData = new List<T>();
        long totalItems = await FetchCountAsync(_httpClient, endpoint);
        const int batchSize = 500;

        for (int offset = 0; offset < totalItems; offset += batchSize)
        {
            var requestBody = $"fields {fields}; limit {batchSize}; offset {offset};";
            var url = $"https://api.igdb.com/v4/{endpoint}";

            var response = await _httpClient.PostAsync(url,
                new StringContent(requestBody, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var batchData = await response.Content.ReadFromJsonAsync<T[]>();
            Console.WriteLine(batchData);
            if (batchData.Length > 0)
            {
                allData.AddRange(batchData);
            }

            await Task.Delay(500);
        }

        return allData;
    }

    public async Task<long> FetchCountAsync(HttpClient client, string endpoint)
    {
        var url = $"{endpoint}/count";
        var response = await client.PostAsync(url, null);
        response.EnsureSuccessStatusCode();

        var countResult = await response.Content.ReadFromJsonAsync<CountResponse>();
        return countResult?.Count ?? 0;
    }
}