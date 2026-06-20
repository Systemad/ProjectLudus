namespace CatalogAPI.Features.IGDB;

public static class Map
{
    public static IEndpointRouteBuilder MapIGDBFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/igdb").CacheOutput("DefaultCache");

        group
            .MapGet("/most-anticipated", GetMostAnticipated.Endpoint.HandleAsync)
            .WithName("IGDB/GetMostAnticipated")
            .WithTags(EndpointMetadata.IGDB)
            .Produces<GetMostAnticipated.AnticipatedGamesResponse>();

        group
            .MapGet("/popscore", GetPopscore.Endpoint.HandleAsync)
            .WithName("IGDB/GetPopscore")
            .WithTags(EndpointMetadata.IGDB)
            .Produces<GetPopscore.GetPopscoreResponse>();

        group
            .MapGet("/stats", GetStatistics.Endpoint.HandleAsync)
            .WithName("IGDB/GetStats")
            .WithTags(EndpointMetadata.IGDB)
            .Produces<GetStatistics.StatisticsResponse>();

        return app;
    }
}
