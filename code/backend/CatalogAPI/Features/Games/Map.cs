using CatalogAPI.Features.Games.Get;
using CatalogAPI.Features.Games.GetByReleaseDateRange;
using CatalogAPI.Features.Games.GetGamePageReleaseData;
using CatalogAPI.Features.Games.GetMedia;
using CatalogAPI.Features.Games.GetOverview;
using CatalogAPI.Features.Games.GetSimilarGames;

namespace CatalogAPI.Features.Games;

public static class Map
{
    public static IEndpointRouteBuilder MapGamesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/games").CacheOutput("DefaultCache");

        group
            .MapGet("/release-date-range", GetByReleaseDateRangeEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseDateRange")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetByReleaseDateRangeEndpoint.Response>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/{gameId:long}", GetGameOverviewEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetOverview")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameOverviewEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/details", GetGameEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/Get")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/media", GetGameMediaEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetMedia")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameMediaEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/page-release-data", GetGamePageReleaseDataEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseData")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGamePageReleaseDataEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/similar-games", GetSimilarGamesEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetSimilar")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetSimilarGamesEndpoint.Response>(StatusCodes.Status200OK);

        return app;
    }
}
