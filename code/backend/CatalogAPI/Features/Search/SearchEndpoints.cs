using CatalogAPI.Context;
using CatalogAPI.Data;
using CatalogAPI.Features.Tags;
using CatalogAPI.Models;
using EFCore.ParadeDB.PgSearch;
using EFCore.ParadeDB.PgSearch.Internals.Aggregates;
using Jameak.CursorPagination;
using Jameak.CursorPagination.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Search;

public record SearchRequest
{
    public string? Name { get; init; }
    public string[]? Genres { get; init; }
    public string[]? Themes { get; init; }
    public string[]? GameModes { get; init; }
    public string[]? Multiplayer { get; init; }
    public string[]? Perspectives { get; init; }
    public int Page { get; init; } = 1;
    public int Limit { get; init; } = 40;
}

/// <summary>
///  REQUESTS HAVE TO BE LOWERCASE!!!!, SO, REQ.NAME.LOWERCASE, REQ.GENRE.LOWERCASE
/// </summary>
public static class SearchEndpoints
{
    public static IEndpointRouteBuilder UseSearchEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/search");

        group
            .MapGet(
                "/",
                async (
                    [AsParameters] SearchRequest req,
                    HttpRequest request,
                    AppDbContext dbContext,
                    OffsetPaginationStrategy _offsetPaginationStrategy,
                    CancellationToken token
                ) =>
                {
                    IQueryable<GamesSearch> query = dbContext.GamesSearches.AsQueryable();
                    IQueryable<GamesSearch> aggregatesQuery = dbContext.GamesSearches.AsQueryable();

                    bool hasSearch = false;
                    if (!string.IsNullOrWhiteSpace(req.Name))
                    {
                        query = query.Where(g =>
                            EF.Functions.MatchDisjunction(g.Name, req.Name.ToLower())
                        );
                        aggregatesQuery = aggregatesQuery.Where(g =>
                            EF.Functions.MatchDisjunction(g.Name, req.Name.ToLower())
                        );
                        hasSearch = true;
                    }

                    if (req.Themes is { Length: > 0 })
                    {
                        foreach (var term in req.Themes)
                        {
                            query = query.Where(p => EF.Functions.Term(p.Themes, term.ToLower()));
                            aggregatesQuery = aggregatesQuery.Where(p =>
                                EF.Functions.Term(p.Themes, term.ToLower())
                            );
                        }
                        hasSearch = true;
                    }

                    if (req.GameModes is { Length: > 0 })
                    {
                        foreach (var term in req.GameModes)
                        {
                            query = query.Where(p =>
                                EF.Functions.Term(p.GameModes, term.ToLower())
                            );
                            aggregatesQuery = aggregatesQuery.Where(p =>
                                EF.Functions.Term(p.GameModes, term.ToLower())
                            );
                        }
                        hasSearch = true;
                    }
                    if (req.Genres is { Length: > 0 })
                    {
                        foreach (var term in req.Genres)
                        {
                            query = query.Where(p => EF.Functions.Term(p.Genres, term.ToLower()));
                            aggregatesQuery = aggregatesQuery.Where(p =>
                                EF.Functions.Term(p.Genres, term.ToLower())
                            );
                        }
                        hasSearch = true;
                    }

                    if (req.Multiplayer is { Length: > 0 })
                    {
                        foreach (var term in req.Multiplayer)
                        {
                            query = query.Where(p =>
                                EF.Functions.Term(p.MultiplayerModes, term.ToLower())
                            );
                            aggregatesQuery = aggregatesQuery.Where(p =>
                                EF.Functions.Term(p.MultiplayerModes, term.ToLower())
                            );
                        }
                        hasSearch = true;
                    }

                    if (req.Perspectives is { Length: > 0 })
                    {
                        foreach (var term in req.Perspectives)
                        {
                            query = query.Where(p =>
                                EF.Functions.Term(p.PlayerPerspectives, term.ToLower())
                            );
                            aggregatesQuery = aggregatesQuery.Where(p =>
                                EF.Functions.Term(p.PlayerPerspectives, term.ToLower())
                            );
                        }
                        hasSearch = true;
                    }

                    var rawAggregates = await aggregatesQuery
                        .Where(g => EF.Functions.All(g.Id))
                        .Select(x => new
                        {
                            Genres = EF.Functions.Aggregate(
                                x.Themes,
                                new TermsAggregate("genres")
                                {
                                    MinDocCount = 0,
                                    Size = 25,
                                    Order = "asc",
                                }
                            ),
                            Themes = EF.Functions.Aggregate(
                                x.Genres,
                                new TermsAggregate("themes")
                                {
                                    MinDocCount = 0,
                                    Size = 25,
                                    Order = "asc",
                                }
                            ),
                            GameModes = EF.Functions.Aggregate(
                                x.GameModes,
                                new TermsAggregate("game_modes")
                                {
                                    MinDocCount = 0,
                                    Size = 25,
                                    Order = "asc",
                                }
                            ),
                            MultiPlayerModes = EF.Functions.Aggregate(
                                x.GameModes,
                                new TermsAggregate("multiplayer_modes")
                                {
                                    MinDocCount = 0,
                                    Size = 25,
                                    Order = "asc",
                                }
                            ),
                            PlayerPerspective = EF.Functions.Aggregate(
                                x.GameModes,
                                new TermsAggregate("player_perspectives")
                                {
                                    MinDocCount = 0,
                                    Size = 25,
                                    Order = "asc",
                                }
                            ),
                            Total = EF.Functions.Count(x.Id),
                        })
                        .FirstOrDefaultAsync(cancellationToken: token);

                    var aggregates = rawAggregates is null
                        ? null
                        : new SearchAggregateResult(
                            rawAggregates.Genres,
                            rawAggregates.Themes,
                            rawAggregates.GameModes,
                            rawAggregates.MultiPlayerModes,
                            rawAggregates.PlayerPerspective,
                            rawAggregates.Total
                        );

                    var aggregationBuckets = FacetGroupBuilder.Build(req, aggregates);
                    IQueryable<GameSearchFacet> rows;

                    if (hasSearch)
                    {
                        rows = query
                            .Select(sub => new GameSearchFacet
                            {
                                Id = (long)sub.Id!,
                                Name = sub.Name,
                                Summary = sub.Summary,
                                Storyline = sub.Storyline,
                                FirstReleaseDate = sub.FirstReleaseDate,
                                GameType = sub.GameType,
                                GameStatus = sub.GameStatus,
                                CoverUrl = sub.CoverUrl,
                                ReleaseYear = sub.ReleaseYear,
                                Score = EF.Functions.Score(sub.Id),
                                Themes = sub.Themes,
                                Genres = sub.Genres,
                                GameModes = sub.GameModes,
                                Platforms = sub.Platforms,
                                GameEngines = sub.GameEngines,
                                PlayerPerspectives = sub.PlayerPerspectives,
                                Publishers = sub.Publishers,
                                Developers = sub.Developers,
                                MultiplayerModes = sub.MultiplayerModes,
                            })
                            .OrderByDescending(x => x.Score);
                    }
                    else
                    {
                        rows = query
                            .Select(sub => new GameSearchFacet
                            {
                                Id = (long)sub.Id!,
                                Name = sub.Name,
                                Summary = sub.Summary,
                                Storyline = sub.Storyline,
                                FirstReleaseDate = sub.FirstReleaseDate,
                                GameType = sub.GameType,
                                GameStatus = sub.GameStatus,
                                CoverUrl = sub.CoverUrl,
                                ReleaseYear = sub.ReleaseYear,
                                Score = 0,
                                Themes = sub.Themes,
                                Genres = sub.Genres,
                                GameModes = sub.GameModes,
                                Platforms = sub.Platforms,
                                GameEngines = sub.GameEngines,
                                PlayerPerspectives = sub.PlayerPerspectives,
                                Publishers = sub.Publishers,
                                Developers = sub.Developers,
                                MultiplayerModes = sub.MultiplayerModes,
                            })
                            .OrderByDescending(x => x.Id);
                    }

                    var page = await OffsetPaginator.ApplyPaginationAsync(
                        strategy: _offsetPaginationStrategy,
                        queryable: rows,
                        asyncMaterializationFunc: DelegateMethods.ToListAsyncDelegate(),
                        asyncCountFunc: DelegateMethods.CountAsyncDelegate(),
                        pageNumber: req.Page,
                        pageSize: 40,
                        computeTotalCount: ComputeTotalCount.Never,
                        computeNextPage: ComputeNextPage.EveryPageAndPreventNextPageQueryOnLastPage,
                        cancellationToken: token
                    );

                    var pageMetadata = new PageMetadata
                    {
                        HasNextPage = page.HasNextPage!.Value,
                        HasPreviousPage = await page.HasPreviousPageAsync(),
                        NextPageCursor = page.NextCursor?.CursorToString(),
                    };

                    var data = page
                        .Items.Select(item => new PagedItem<GameItem>()
                        {
                            Cursor = item.Cursor.CursorToString(),
                            Item = item.Data.MapTo(),
                        })
                        .ToList();

                    return new SearchResponse<GameItem>(
                        TotalCount: aggregates?.Total ?? 0,
                        PageSize: req.Limit,
                        Data: data,
                        AggregationBuckets: aggregationBuckets,
                        PageMetadata: pageMetadata
                    );
                }
            )
            .WithName("Search");
        return group;
    }
}
