using CatalogAPI.Features.Companies.Get;
using CatalogAPI.Features.Companies.GetGames;

namespace CatalogAPI.Features.Companies;

public static class Map
{
    public static IEndpointRouteBuilder MapCompaniesFeature(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").CacheOutput("DefaultCache");

        group.MapGetCompanyEndpoint();
        group.MapGetCompanyGamesEndpoint();

        return app;
    }
}