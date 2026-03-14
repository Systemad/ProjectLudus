using System.Text.Json;
using CatalogAPI.Context;
using CatalogAPI.Data;
using CatalogAPI.Features.Tags;
using CatalogAPI.Models;
using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Internals.Aggregates;
using Jameak.CursorPagination;
using Jameak.CursorPagination.Enums;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Search;

public static class SearchEndpoints
{
    public static IEndpointRouteBuilder UseSearchEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/search");

        group
            .MapGet(
                "/",
                async (
                    [AsParameters] GameSearchRequest req,
                    HttpRequest request,
                    CancellationToken token,
                    AppDbContext dbContext,
                    KeySetPaginationStrategy _keySetPaginationStrategy
                ) =>
                {
                    IQueryable<GamesSearch> query = dbContext.GamesSearches.AsQueryable();

                    var heyhey = await dbContext.GamesSearches.LongCountAsync();
                    
                    IQueryable<GamesSearch> aggregatesQuery = dbContext.GamesSearches.AsQueryable();
                    bool hasSearch = false;
                    if (!string.IsNullOrWhiteSpace(req.Name))
                    {
                        query = query.Where(g => EF.Functions.MatchDisjunction(g.Name, req.Name));
                        aggregatesQuery = aggregatesQuery.Where(g =>
                            EF.Functions.MatchDisjunction(g.Name, req.Name)
                        );
                        hasSearch = true;
                    }

                    if (req.Themes is { Length: > 0 })
                    {
                        foreach (var term in req.Themes)
                        {
                            query = query.Where(p => EF.Functions.Term(p.Themes, term));
                            aggregatesQuery = aggregatesQuery.Where(p => EF.Functions.Term(p.Themes, term));
                        }
                        hasSearch = true;
                    }

                    if (req.Modes is { Length: > 0 })
                    {
                        foreach (var term in req.Modes)
                        {
                            query = query.Where(p => EF.Functions.Term(p.Modes, term));
                            aggregatesQuery = aggregatesQuery.Where(p => EF.Functions.Term(p.Modes, term));
                        }
                        hasSearch = true;
                    }
                    if (req.Genres is { Length: > 0 })
                    {
                        foreach (var term in req.Genres)
                        {
                            query = query.Where(p => EF.Functions.Term(p.Genres, term));
                            aggregatesQuery = aggregatesQuery.Where(p => EF.Functions.Term(p.Genres, term));
                        }
                        hasSearch = true;
                    }
                    var aggregates = await aggregatesQuery
                        .Where(g => EF.Functions.All(g.Id))
                        .Select(x => new
                        {
                            Genres = EF.Functions.Aggregate(x.Themes, new TermsAggregate("genres"){MinDocCount = 0, Size = 25, Order = "asc"}),
                            Themes = EF.Functions.Aggregate(x.Themes, new TermsAggregate("themes"){MinDocCount = 0, Size = 25, Order = "asc"}),
                            Modes = EF.Functions.Aggregate(x.Themes, new TermsAggregate("modes"){MinDocCount = 0, Size = 25, Order = "asc"}),
                            Total = EF.Functions.Count(x.Id)
                        })
                        .FirstOrDefaultAsync(cancellationToken: token);

                    // TODO: Handle later
                    var aggregationBucketsDictionary = new Dictionary<string, AggregationBuckets>
                    {
                        [TagKeys.GENRES] = JsonSerializer.Deserialize<AggregationBuckets>(aggregates!.Genres)!,
                        [TagKeys.THEMES] = JsonSerializer.Deserialize<AggregationBuckets>(aggregates!.Themes)!,
                        [TagKeys.MODES] = JsonSerializer.Deserialize<AggregationBuckets>(aggregates!.Modes)!,
                        //["platforms"] = JsonSerializer.Deserialize<AggregationBuckets>(agg!.Platforms)!,
                        //["developers"] = JsonSerializer.Deserialize<AggregationBuckets>(agg!.Developers)!
                    };
                    IQueryable<GameSearchFacet> rows;
                    
                    if (hasSearch)
                    {
                        rows = query.Select(sub => new GameSearchFacet
                            {
                                Id = (long)sub.Id!,
                                Name = sub.Name,
                                Summary = sub.Summary,
                                Storyline = sub.Storyline,
                                FirstReleaseDate = sub.FirstReleaseDate,
                                GameType = sub.GameType,
                                CoverUrl = sub.CoverUrl,
                                ReleaseYear = sub.ReleaseYear,
                                Score = EF.Functions.Score(sub.Id),
                                Themes = sub.Themes,
                                Genres = sub.Genres,
                                Modes = sub.Modes,
                            })
                            .OrderByDescending(x => x.Score);
                    }
                    else
                    {
                        rows = query.Select(sub => new GameSearchFacet
                            {
                                Id = (long)sub.Id!,
                                Name = sub.Name,
                                Summary = sub.Summary,
                                Storyline = sub.Storyline,
                                FirstReleaseDate = sub.FirstReleaseDate,
                                GameType = sub.GameType,
                                CoverUrl = sub.CoverUrl,
                                ReleaseYear = sub.ReleaseYear,
                                Score = 0,
                                Themes = sub.Themes,
                                Genres = sub.Genres,
                                Modes = sub.Modes,
                            })
                            .OrderByDescending(x => x.Id);
                    }

                    var page = await KeySetPaginator.ApplyPaginationAsync<
                        GameSearchFacet,
                        KeySetPaginationStrategy.Cursor,
                        KeySetPaginationStrategy
                    >(
                        _keySetPaginationStrategy,
                        rows,
                        DelegateMethods.ToListAsyncDelegate(),
                        DelegateMethods.CountAsyncDelegate(),
                        DelegateMethods.AnyAsyncDelegate(),
                        req.AfterCursor,
                        40,
                        //paginationDirection: PaginationDirection.Forward,
                        computeTotalCount: ComputeTotalCount.Never,
                        computeNextPage: ComputeNextPage.EveryPageAndPreventNextPageQueryOnLastPage,
                        cancellationToken: token
                    );

                    var pageMetadata = new PageMetadata
                    {
                        NextPageCursor =
                            page.NextCursor == null
                                ? null
                                : _keySetPaginationStrategy.CursorToString(page.NextCursor),
                        HasNextPage = page.HasNextPage!.Value,
                        HasPreviousPage = await page.HasPreviousPageAsync(),
                    };
                    var data = page
                        .Items.Select(item => new PagedItem<GameItem>()
                        {
                            Cursor = _keySetPaginationStrategy.CursorToString(item.Cursor),
                            Item = item.Data.MapTo(),
                        })
                        .ToList();
                 
                    return new SearchResponse<GameItem>(
                        TotalCount: aggregates.Total,
                        PageSize: req.PageSize,
                        Data: data,
                        AggregationBuckets: aggregationBucketsDictionary,
                        PageMetadata: pageMetadata
                    );
                }
            )
            .WithName("Search");
        return group;
    }
}
