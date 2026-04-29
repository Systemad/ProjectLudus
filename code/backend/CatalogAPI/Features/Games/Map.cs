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
        var group = app.MapGroup("/catalog/games").CacheOutput("DefaultCache");

        group
            .MapGet("/release-date-range", GetByReleaseDateRangeEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseDateRange")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetByReleaseDateRangeEndpoint.GetByReleaseDateRangeResponse>(
                StatusCodes.Status200OK
            )
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/{gameId:long}", GetGameOverviewEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetOverview")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameOverviewEndpoint.GetGameOverviewResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/details", GetGameEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/Get")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameEndpoint.GetGameResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/media", GetGameMediaEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetMedia")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGameMediaEndpoint.GetGameMediaResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/page-release-data", GetGamePageReleaseDataEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseData")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetGamePageReleaseDataEndpoint.GetGamePageReleaseDataResponse>(
                StatusCodes.Status200OK
            )
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId:long}/similar-games", GetSimilarGamesEndpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetSimilar")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetSimilarGamesEndpoint.GetSimilarGamesResponse>(StatusCodes.Status200OK);

        return app;
    }
}
