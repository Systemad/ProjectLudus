using CatalogAPI.Features.Games.GetById;
using CatalogAPI.Features.Games.GetByReleaseDateRange;
using CatalogAPI.Features.Games.GetGamePageReleaseData;
using CatalogAPI.Features.Games.GetMediaById;
using CatalogAPI.Features.Games.GetOverviewById;
using CatalogAPI.Features.Games.GetSimilairGames;

namespace CatalogAPI.Features.Games;

public static class GamesEndpoints
{
    public static IEndpointRouteBuilder UseGamesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetByReleaseDateRangeEndpoints();
        app.MapGetOverviewByIdEndpoints();
        app.MapGetByIdEndpoints();
        app.MapGetMediaByIdEndpoints();
        app.MapGetGamePageReleaseDataEndpoints();
        app.MapGetSimilairGamesEndpoints();
        return app;
    }
}
