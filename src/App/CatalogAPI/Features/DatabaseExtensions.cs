using CatalogAPI.Data;
using Microsoft.EntityFrameworkCore;
using PhenX.EntityFrameworkCore.BulkInsert.Extensions;
using PhenX.EntityFrameworkCore.BulkInsert.Options;

namespace CatalogAPI.Features;

public static class DatabaseExtensions
{
    public static Task BulkUpsertAsync<T>(
        this CatalogContext dbContext,
        List<T> source,
        CancellationToken ct
    )
        where T : class
    {
        if (source.Count == 0)
        {
            return Task.CompletedTask;
        }

        return dbContext.ExecuteBulkInsertAsync(
            source,
            o => { },
            new OnConflictOptions<T> { Update = (inserted, excluded) => inserted },
            ct
        );
    }

    public static Task BulkInsertAsync<T>(
        this DbContext dbContext,
        List<T> source,
        CancellationToken ct
    )
        where T : class
    {
        if (source.Count == 0)
        {
            return Task.CompletedTask;
        }

        return dbContext.ExecuteBulkInsertAsync(source, cancellationToken: ct);
    }
}
