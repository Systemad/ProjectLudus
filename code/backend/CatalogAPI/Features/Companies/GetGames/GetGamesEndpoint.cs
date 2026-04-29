using CatalogAPI.Features.Games.Common.Projections;

namespace CatalogAPI.Features.Companies.GetGames;

public static class GetCompanyGamesEndpoint
{
    public record GetCompanyGamesResponse(CompanyGamesDto CompanyGames);

    public static async Task<IResult> HandleAsync(
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
                db.Games,
                ic => ic.Game,
                game => game.Id,
                (ic, game) =>
                    new
                    {
                        Game = game,
                        ic.Publisher,
                        ic.Developer,
                    }
            )
            .GroupBy(g => g.Game.Id)
            .Select(group => new
            {
                Game = group
                    .Select(g => g.Game)
                    .AsQueryable()
                    .Select(GameDtoProjection.AsGameDto)
                    .First(),
                IsPublisher = group.Any(g => g.Publisher == true),
                IsDeveloper = group.Any(g => g.Developer == true),
            })
            .ToListAsync(cancellationToken);

        var publishedGames = games.Where(g => g.IsPublisher).Select(g => g.Game).ToList();

        var developedGames = games.Where(g => g.IsDeveloper).Select(g => g.Game).ToList();

        return Results.Ok(
            new GetCompanyGamesResponse(new CompanyGamesDto(publishedGames, developedGames))
        );
    }
}
