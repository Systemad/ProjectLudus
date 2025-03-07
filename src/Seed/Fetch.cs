using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Shared;

namespace Seed;

public class Fetch
{
    private readonly HttpClient _httpClient;

    public Fetch(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Fetch total count from the /count endpoint
    public async Task<List<T>> FetchAllDataAsync<T>(string endpoint, string fields, CancellationToken cancellationToken)
    {
        /*
            _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
            _httpClient.DefaultRequestHeaders.Add("Client-ID","");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer ");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        */
        var allData = new List<T>();
        long totalItems = await FetchCountAsync(_httpClient, endpoint, cancellationToken);
        const int batchSize = 500;

        for (int offset = 0; offset < totalItems; offset += batchSize)
        {
            var requestBody = $"fields {fields}; limit {batchSize}; offset {offset};";
            var url = $"https://api.igdb.com/v4/{endpoint}";

            var response = await _httpClient.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"), cancellationToken);
            response.EnsureSuccessStatusCode();

            var batchData = await response.Content.ReadFromJsonAsync<T[]>(cancellationToken);
        
            if (batchData.Length > 0)
            {
                allData.AddRange(batchData);
            }

            await Task.Delay(500, cancellationToken);
        }

        return allData;
    }
    
    public async Task<long> FetchCountAsync(HttpClient client, string endpoint, CancellationToken cancellationToken)
    {
        var url = $"https://api.igdb.com/v4/{endpoint}/count";
        var response = await client.PostAsync(url, new StringContent(""), cancellationToken);
        response.EnsureSuccessStatusCode();

        var countResult = await response.Content.ReadFromJsonAsync<CountResponse>(cancellationToken);
        return countResult?.Count ?? 0;
    }
    
}