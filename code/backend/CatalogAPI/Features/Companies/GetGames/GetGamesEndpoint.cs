namespace CatalogAPI.Features.Companies.GetGames;

public static class GetGamesEndpoint
{
    public record GetCompanyGamesResponse(CompanyGamesDto CompanyGames);

    public static IEndpointRouteBuilder MapGetGamesEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("/{companyId:long}/games", GetCompanyGamesAsync)
            .Produces<GetCompanyGamesResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetCompanyGamesAsync(
        long companyId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        if (!await db.Companies.AnyAsync(c => c.Id == companyId, cancellationToken))
            return Results.NotFound();

        var games = await db
            .InvolvedCompanies.Where(ic =>
                ic.Company == companyId && (ic.Publisher == true || ic.Developer == true)
            )
            .Join(
                db.GamesSearches,
                ic => ic.Game,
                search => search.Id,
                (ic, search) =>
                    new
                    {
                        Search = search,
                        ic.Publisher,
                        ic.Developer,
                    }
            )
            .GroupBy(g => g.Search.Id)
            .Select(group => new
            {
                Search = group.Select(g => g.Search).First(),
                IsPublisher = group.Any(g => g.Publisher == true),
                IsDeveloper = group.Any(g => g.Developer == true),
            })
            .ToListAsync(cancellationToken);

        var publishedGames = games.Where(g => g.IsPublisher).Select(g => g.Search).ToList();

        var developedGames = games.Where(g => g.IsDeveloper).Select(g => g.Search).ToList();

        return Results.Ok(
            new GetCompanyGamesResponse(
                new CompanyGamesDto(
                    GameSearchMapper.MapToDto(publishedGames),
                    GameSearchMapper.MapToDto(developedGames)
                )
            )
        );
    }
}
