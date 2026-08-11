using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Steam;

public static class Map
{
    public static IEndpointRouteBuilder MapSteamFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/steam").CacheOutput("SteamCache");

        group
            .MapGet("/chart", Charts.GetChart.Endpoint.HandleAsync)
            .WithName("Steam/Chart")
            .WithTags(EndpointMetadata.Steam)
            .Produces<PagedGamesResponse>();

        group
            .MapGet(
                "/concurrent-users/{gameId}/chart",
                Charts.GetConcurrentUsersChart.Endpoint.HandleChartAsync
            )
            .WithName("Steam/GetConcurrentUsersChart")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Charts.GetConcurrentUsersChart.ConcurrentUsersChartResponse>()
            .Produces(StatusCodes.Status400BadRequest);

        group
            .MapGet("/pricing/{gameId}", Store.GetPricing.Endpoint.HandleAsync)
            .WithName("Steam/GetPricing")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Store.GetPricing.GetPricingResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/reviews/{gameId}", Store.GetReviews.Endpoint.HandleAsync)
            .WithName("Steam/GetReviews")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Store.GetReviews.GetReviewsResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
