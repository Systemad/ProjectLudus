namespace Catalog.Features.Companies.GetGames;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        string companyId,
        ICompanyService companyService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(companyId, out var parsedCompanyId))
            return Results.BadRequest();

        var games = await companyService.GetGamesAsync(parsedCompanyId, cancellationToken);
        return Results.Ok(games);
    }
}
