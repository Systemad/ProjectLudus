using Shared.Features.Games;
using Shared.Queries;
using SyncService.Cache;
using SyncService.Services;

namespace SyncService.Seed;

public class CompanySeeder(ApiClient apiClient, CompanyDatabaseService companyDatabaseService)
{
    public async Task PopulateCompaniesAsync(bool writeToCache = false, bool reset = false)
    {
        if (reset)
        {
        }

        var countResponse = await apiClient.FetchCountAsync(CompanyQuery.Url);

        int batchSize = 500;

        var allCompanies = new List<Company>();
        for (var offset = 0; offset < countResponse.Count; offset += batchSize)
        {
            var take = Math.Min(batchSize, countResponse.Count - offset);

            Console.WriteLine($"Fetching {take} companies!");
            
            var companies = await apiClient.FetchBatchAsyncGeneric<Company>(
                CompanyQuery.Url,
                CompanyQuery.Fields,
                take,
                offset
            );
            
            allCompanies.AddRange(companies);
            await Task.Delay(200);
        }

        await companyDatabaseService.InsertCompaniesAsync(allCompanies);
        if (writeToCache)
        {
            await OptimizedList.WriteToCacheFileAsync(allCompanies, FilePath.COMPANIES);
            //await WriteToJsonCacheCompanyAsync(allCompanies);
        }
    }

}