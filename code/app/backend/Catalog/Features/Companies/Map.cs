using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Companies;

public static class Map
{
    public static IEndpointRouteBuilder MapCompaniesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/catalog/companies").CacheOutput("DefaultCache");

        group
            .MapGet("/{companyId}", Get.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Companies}/Get")
            .WithTags(EndpointMetadata.Companies)
            .Produces<Get.GetCompanyResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{companyId}/games", GetGames.Endpoint.HandleAsync)
            .WithName($"{EndpointMetadata.Companies}/GetGames")
            .WithTags(EndpointMetadata.Companies)
            .Produces<List<GameBrowseDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
