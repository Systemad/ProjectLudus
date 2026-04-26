using CatalogAPI.Features.Companies.Get;
using CatalogAPI.Features.Companies.GetGames;

namespace CatalogAPI.Features.Companies;

public static class Map
{
    public static IEndpointRouteBuilder MapCompaniesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").CacheOutput("DefaultCache");

        group
            .MapGet("/{companyId:long}", GetCompanyEndpoint.HandleAsync)
            .WithName("Companies/Get")
            .WithTags("Companies")
            .Produces<GetCompanyEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group
            .MapGet("/{companyId:long}/games", GetCompanyGamesEndpoint.HandleAsync)
            .WithName("Companies/GetGames")
            .WithTags("Companies")
            .Produces<GetCompanyGamesEndpoint.Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
