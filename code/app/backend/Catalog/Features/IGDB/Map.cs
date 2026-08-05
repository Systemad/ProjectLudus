using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.IGDB;

public static class Map
{
    public static IEndpointRouteBuilder MapIGDBFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/igdb").CacheOutput("DefaultCache");

        group
            .MapGet("/most-anticipated", GetMostAnticipated.Endpoint.HandleAsync)
            .WithName("Igdb/GetMostAnticipated")
            .WithTags(EndpointMetadata.IGDB)
            .Produces<PagedGamesResponse>();

        group
            .MapGet("/popscore", GetPopscore.Endpoint.HandleAsync)
            .WithName("Igdb/GetPopscore")
            .WithTags(EndpointMetadata.IGDB)
            .Produces<PagedGamesResponse>()
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group
            .MapGet("/stats", GetStatistics.Endpoint.HandleAsync)
            .WithName("Igdb/GetStats")
            .WithTags(EndpointMetadata.IGDB)
            .Produces<GetStatistics.StatisticsResponse>();

        return app;
    }
}
