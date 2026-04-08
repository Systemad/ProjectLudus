using CatalogAPI.Features.Games.GetByReleaseDateRange;

namespace CatalogAPI.Features.Games;

public static class GamesEndpoints
{
    public static IEndpointRouteBuilder UseGamesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetByReleaseDateRangeEndpoints();
        return app;
    }
}
