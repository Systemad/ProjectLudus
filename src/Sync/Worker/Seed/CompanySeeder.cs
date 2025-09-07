using System.Text.Json;
using Shared.Features.Games;
using Shared.Queries;

namespace Worker.Seed;

public class CompanySeeder
{

    private ApiClient _apiClient;

    private const string companyFile = "Cache/companies.json";
    
    public CompanySeeder(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    
    public async Task PopulateCompaniesAsync(bool writeToCache = false, bool reset = false)
    {
        
        if (reset)
        {
        }
        
        var countResponse = await _apiClient.FetchCountAsync(CompanyQuery.Url);

        int maxItemsPerIteration = 500;
        long totalItems = countResponse.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

        var allCompanies = new List<Company>();
        for (long i = 0; i < iterations; i++)
        {
            long offset = i * maxItemsPerIteration;
            long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

            Console.WriteLine($"Fetching {itemsToTake} companies!");
            await InsertCompanyBatchAsync(itemsToTake, offset);
            await Task.Delay(200);
        }

        if (writeToCache)
        {
            await WriteToJsonCacheCompanyAsync(allCompanies);
        }
        
    }
    
    private async Task InsertCompanyBatchAsync(long itemsToTake, long offset)
    {
        var companies = await _apiClient.FetchBatchAsyncGeneric<Company>(
            CompanyQuery.Url,
            CompanyQuery.Fields,
            itemsToTake,
            offset
        );


    }
    
    private static async Task WriteToJsonCacheCompanyAsync(List<Company> companies)
    {
        await using var stream = new StreamWriter(companyFile, append: true);
        var options = new JsonSerializerOptions { WriteIndented = false };
        foreach (var company in companies)
        {
            string json = JsonSerializer.Serialize(company, options);
            await stream.WriteLineAsync(json);
        }
    }
}