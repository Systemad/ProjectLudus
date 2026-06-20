using CatalogAPI.Features.Steam.Charts.GetChart;
using CatalogAPI.Features.Steam.Charts.GetConcurrentUsersChart;
using CatalogAPI.Features.Steam.Store.GetPricing;
using CatalogAPI.Features.Steam.Store.GetReviews;

namespace CatalogAPI.Features.Steam;

public static class Map
{
    public static IEndpointRouteBuilder MapSteamFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/steam").CacheOutput("SteamCache");

        group
            .MapGet("/chart", Charts.GetChart.Endpoint.HandleAsync)
            .WithName("Steam/Chart")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Charts.GetChart.GamesResponse>();

        group
            .MapGet("/concurrent-users/{gameId:long}/chart", Charts.GetConcurrentUsersChart.Endpoint.HandleChartAsync)
            .WithName("Steam/GetConcurrentUsersChart")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Charts.GetConcurrentUsersChart.ConcurrentUsersChartResponse>();

        group
            .MapGet("/pricing/{gameId:long}", Store.GetPricing.Endpoint.HandleAsync)
            .WithName("Steam/GetPricing")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Store.GetPricing.GetPricingResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/reviews/{gameId:long}", Store.GetReviews.Endpoint.HandleAsync)
            .WithName("Steam/GetReviews")
            .WithTags(EndpointMetadata.Steam)
            .Produces<Store.GetReviews.GetReviewsResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
