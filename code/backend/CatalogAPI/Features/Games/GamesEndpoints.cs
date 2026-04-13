using CatalogAPI.Features.Games.Get;
using CatalogAPI.Features.Games.GetGamePageReleaseData;
using CatalogAPI.Features.Games.GetMedia;
using CatalogAPI.Features.Games.GetOverview;
using CatalogAPI.Features.Games.GetSimilarGames;

namespace CatalogAPI.Features.Games;

public static class GamesEndpoints
{
    public static IEndpointRouteBuilder UseGamesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/games").CacheOutput("DefaultCache");

        group.MapGetByReleaseDateRangeEndpoint();
        group.MapGetOverviewEndpoint();
        group.MapGetEndpoint();
        group.MapGetMediaEndpoint();
        group.MapGetGamePageReleaseDataEndpoint();
        group.MapGetSimilarGamesEndpoint();

        return app;
    }
}
