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

        group.MapGetGamesByReleaseDateRangeEndpoint();
        group.MapGetGameOverviewEndpoint();
        group.MapGetGameEndpoint();
        group.MapGetGameMediaEndpoint();
        group.MapGetGameReleaseDataEndpoint();
        group.MapGetSimilarGamesEndpoint();

        return app;
    }
}
