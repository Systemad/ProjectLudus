using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Companies.GetGames;

public static class Endpoint
{
    public static async Task<Results<BadRequest, Ok<List<GameBrowseDto>>>> HandleAsync(
        string companyId,
        ICompanyService companyService,
        CancellationToken cancellationToken
    )
    {
        if (!ApiId.TryParse(companyId, out var parsedCompanyId))
            return TypedResults.BadRequest();

        var games = await companyService.GetGamesAsync(parsedCompanyId, cancellationToken);
        return TypedResults.Ok(games);
    }
}
