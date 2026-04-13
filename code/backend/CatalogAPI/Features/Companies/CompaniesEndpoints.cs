using CatalogAPI.Features.Companies.Get;
using CatalogAPI.Features.Companies.GetGames;

namespace CatalogAPI.Features.Companies;

public static class CompaniesEndpoints
{
    public static IEndpointRouteBuilder UseCompaniesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").CacheOutput("DefaultCache");

        group.MapGetEndpoint();
        group.MapGetGamesEndpoint();

        return app;
    }
}
