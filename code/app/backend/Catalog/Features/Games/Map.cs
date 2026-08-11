using Catalog.Features.Games.Browse.GetByReleaseDateRange;
using Catalog.Features.Games.Common.Pagination;
using Endpoint = Catalog.Features.Games.Browse.GetByReleaseDateRange.Endpoint;

namespace Catalog.Features.Games;

public static class Map
{
    public static IEndpointRouteBuilder MapGamesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/games").CacheOutput("DefaultCache");

        group
            .MapGet("/browse", Browse.GetByFilter.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/Browse")
            .WithTags(EndpointMetadata.Games)
            .WithSummary("Browse games")
            .Produces<PagedGamesResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/release-date-range", Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseDateRange")
            .WithTags(EndpointMetadata.Games)
            .Produces<GetByReleaseDateRangeResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/{gameId}", Browse.GetOverview.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetOverview")
            .WithTags(EndpointMetadata.Games)
            .Produces<Browse.GetOverview.GetGameOverviewResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId}/details", Browse.Get.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/Get")
            .WithTags(EndpointMetadata.Games)
            .Produces<Browse.Get.GetGameResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId}/hero", Browse.GetHero.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetHero")
            .WithTags(EndpointMetadata.Games)
            .Produces<Browse.GetHero.GetGameHeroResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId}/links", Browse.GetLinks.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetLinks")
            .WithTags(EndpointMetadata.Games)
            .Produces<Browse.GetLinks.GetGameLinksResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group
            .MapGet("/{gameId}/media", Browse.GetMedia.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetMedia")
            .WithTags(EndpointMetadata.Games)
            .Produces<Browse.GetMedia.GetGameMediaResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId}/page-release-data", Page.GetReleaseData.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetReleaseData")
            .WithTags(EndpointMetadata.Games)
            .Produces<Page.GetReleaseData.GetGamePageReleaseDataResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{gameId}/similar-games", Browse.GetSimilarGames.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Games}/GetSimilar")
            .WithTags(EndpointMetadata.Games)
            .Produces<Browse.GetSimilarGames.GetSimilarGamesResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}
