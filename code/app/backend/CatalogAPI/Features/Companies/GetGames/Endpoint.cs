namespace CatalogAPI.Features.Companies.GetGames;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        long companyId,
        ICompanyService companyService,
        CancellationToken cancellationToken
    )
    {
        var games = await companyService.GetGamesAsync(companyId, cancellationToken);
        return Results.Ok(games);
    }
}
