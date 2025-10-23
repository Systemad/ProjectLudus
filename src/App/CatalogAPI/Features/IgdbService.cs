using IGDB;
using Shared.Features;

namespace CatalogAPI.Features;

public record PagedResult<T>(List<T> Items, long TotalCount);

public class IgdbService(IGDBClient apiClient)
{
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
