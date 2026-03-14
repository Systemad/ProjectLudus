using CatalogAPI.Models;
using Jameak.CursorPagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Search;

public static class DelegateMethods
{
    public static ToListAsync<GameSearchFacet> ToListAsyncDelegate() => (queryable, cancellationToken) => queryable.ToListAsync(cancellationToken);
    public static CountAsync<GameSearchFacet> CountAsyncDelegate() => (queryable, cancellationToken) => queryable.CountAsync(cancellationToken);
    public static AnyAsync<GameSearchFacet> AnyAsyncDelegate() => (queryable, cancellationToken) => queryable.AnyAsync(cancellationToken);
}