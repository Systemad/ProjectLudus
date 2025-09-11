using Shared.Queries;
using SyncService.Cache;

namespace SyncService.Features.Company;

public class CompanySeedingSeedingOrchestrator(ApiClient apiClient, CompanyDatabaseService companyDatabaseService)
{
    public async Task PopulateCompaniesAsync(bool writeToCache = false, bool reset = false)
    {
        if (reset)
        {
        }

        var countResponse = await apiClient.FetchCountAsync(CompanyQuery.Url);

        int batchSize = 500;

        var allCompanies = new List<Shared.Features.Games.Company>();
        for (var offset = 0; offset < countResponse.Count; offset += batchSize)
        {
            var take = Math.Min(batchSize, countResponse.Count - offset);

            Console.WriteLine($"Fetching {take} companies!");
            
            var companies = await apiClient.FetchBatchAsyncGeneric<Shared.Features.Games.Company>(
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