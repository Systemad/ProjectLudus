using Shared.Features;
using Shared.Features.IGDB;
using Shared.Queries;
using SyncService.Features.Games;
using CompanyQuery = SyncService.Features.Companies.CompanyQuery;

namespace SyncService.Features;

public record PagedResult<T>(List<T> Items, long TotalCount);

public class IgdbService(ApiClient apiClient)
{
    public async IAsyncEnumerable<PagedResult<Company>> GetReferenceAsync(IgdbReference reference)
    {
        var totalItems = await apiClient.FetchCountAsync("companies");
        await foreach (var pageResult in GetAllPages<Company>(CompanyQuery.Endpoint, CompanyQuery.Fields, totalItems.Count))
        {
            yield return new PagedResult<Company>(pageResult, totalItems.Count);
        }
    }

    public async IAsyncEnumerable<PagedResult<T>> GenericAsyncV1<T>(IgdbReference reference,
        int pageSize = 500)
    {
        var (url, fields) = IgdbFields.Queries[reference];
        var totalItems = await apiClient.FetchCountAsync(url);
        await foreach (var pageResult in GetAllPages<T>(url, fields, totalItems.Count,
                           pageSize))
        {
            yield return new PagedResult<T>(pageResult, totalItems.Count);
        }
    }
    public async IAsyncEnumerable<PagedResult<T>> GenericAsync<T>(string url,
        List<string> fields,
        int pageSize = 500)
    {
        var totalItems = await apiClient.FetchCountAsync(url);
        await foreach (var pageResult in GetAllPages<T>(url, fields, totalItems.Count,
                           pageSize))
        {
            yield return new PagedResult<T>(pageResult, totalItems.Count);
        }
    }

    public async IAsyncEnumerable<PagedResult<IgdbGame>> GetAllGamesAsync(
        int pageSize = 500)
    {
        var totalItems = await apiClient.FetchCountAsync("games");
        await foreach (var pageResult in GetAllPages<IgdbGame>(GameQuery.Endpoint, CompanyQuery.Fields,
                           totalItems.Count, pageSize))
        {
            yield return new PagedResult<IgdbGame>(pageResult, totalItems.Count);
        }
    }

    public async IAsyncEnumerable<PagedResult<Company>> GetAllCompaniesAsync(
        int pageSize = 500)
    {
        var totalItems = await apiClient.FetchCountAsync("companies");
        await foreach (var pageResult in GetAllPages<Company>(CompanyQuery.Endpoint, CompanyQuery.Fields, totalItems.Count,
                           pageSize))
        {
            yield return new PagedResult<Company>(pageResult, totalItems.Count);
        }
    }

    private async IAsyncEnumerable<List<T>> GetAllPages<T>(
        string url,
        List<string> fields,
        long totalCount,
        int pageSize = 500)
    {
        int currentCount = 0;
        var currentPage = 0;

        do
        {
            var response = await apiClient.FetchBatchAsyncGeneric<T>(url, fields, pageSize, currentPage * pageSize);
            currentCount += response.Count;
            yield return response;
            currentPage++;
        } while (totalCount > currentCount);
    }
}