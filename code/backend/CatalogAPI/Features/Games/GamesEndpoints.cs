using CatalogAPI.Features.Games.GetById;
using CatalogAPI.Features.Games.GetByReleaseDateRange;

namespace CatalogAPI.Features.Games;

public static class GamesEndpoints
{
    public static IEndpointRouteBuilder UseGamesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetByReleaseDateRangeEndpoints();
        app.MapGetByIdEndpoints();
        return app;
    }
}
