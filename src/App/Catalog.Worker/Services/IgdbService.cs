using Catalog.Worker.Queries;
using CatalogAPI.Features;
using IGDB;
using Shared.Features;

namespace Catalog.Worker.Services;

public record PagedResult<T>(List<T> Items, long TotalCount);

public interface IIgdbService
{
    IAsyncEnumerable<PagedResult<T>> FetchAllPagesAsync<T>(IgdbType reference, int pageSize = 500);
}

public class IgdbService(IGDBClient apiClient) : IIgdbService
{
    /*
    public async Task<List<T>> FetchMultiQuery<T>()
    {
        MultiQueryResult<T>[]? response = await apiClient.QueryAsync<MultiQueryResult<T>>("multiquery", "query");
        List<MultiQueryResult<T>> hey = response.ToList();
        List<Theme> themesList = ExtractListByName<object, Theme>(hey, "Themes");
        List<Genre> genresList = ExtractListByName<object, Genre>(hey, "Genres");
        
    }

    public List<TTyped> ExtractListByName<T, TTyped>(List<MultiQueryResult<T>> results, string name)
    {
        MultiQueryResult<T>? queryResult = results.FirstOrDefault(r => r.Name == name);
        return queryResult?.Data as List<TTyped> ?? new List<TTyped>();
    }
    
    public List<T> ExtractData<T>(List<MultiQueryResult<T>> results, string queryName)
    {
        var target = results.FirstOrDefault(r => r.Name == queryName);

        return target.Data;
    }
    */

    public async IAsyncEnumerable<PagedResult<T>> FetchAllPagesAsync<T>(
        IgdbType reference,
        int pageSize = 500
    )
    {
        var (url, fields) = QueryHelper.QueryMaps[reference];
        var totalItems = await apiClient.CountAsync(url);
        await foreach (var pageResult in GetAllPages<T>(url, fields, totalItems.Count, pageSize))
        {
            yield return new PagedResult<T>(pageResult, totalItems.Count);
        }
    }

    private async IAsyncEnumerable<List<T>> GetAllPages<T>(
        string url,
        List<string> fields,
        long totalCount,
        int pageSize = 500
    )
    {
        int currentCount = 0;
        var currentPage = 0;

        do
        {
            var query =
                $"fields {string.Join(",", fields)}; limit {pageSize}; offset {currentPage * pageSize};";

            var response = await apiClient.QueryAsync<T>(url, query);

            currentCount += response.Length;
            yield return response.ToList();
            currentPage++;
        } while (totalCount > currentCount);
    }
}


/*
public class IgdbService(ApiClient apiClient)
{
    public async IAsyncEnumerable<PagedResult<T>> FetchAllPagesAsync<T>(
        IgdbReference reference,
        int pageSize = 500
    )
    {
        var (url, fields) = Endpoints.QueryMaps[reference];
        var totalItems = await apiClient.FetchCountAsync(url);
        await foreach (var pageResult in GetAllPages<T>(url, fields, totalItems.Count, pageSize))
        {
            yield return new PagedResult<T>(pageResult, totalItems.Count);
        }
    }

    private async IAsyncEnumerable<List<T>> GetAllPages<T>(
        string url,
        List<string> fields,
        long totalCount,
        int pageSize = 500
    )
    {
        int currentCount = 0;
        var currentPage = 0;

        do
        {
            var response = await apiClient.FetchBatchAsyncGeneric<T>(
                url,
                fields,
                pageSize,
                currentPage * pageSize
            );
            currentCount += response.Count;
            yield return response;
            currentPage++;
        } while (totalCount > currentCount);
    }
}
*/
