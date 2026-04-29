namespace CatalogAPI.Features.Games.Get;

public static class GetGameEndpoint
{
    public record GetGameResponse(GameDetailsDto Game);

    public static async Task<IResult> HandleAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var gameDetailsDto = await db
            .Games.Where(g => g.Id == gameId)
            .Select(g => new GameDetailsDto
            {
                Id = g.Id,
                Url = g.Url ?? string.Empty,
                InvolvedCompanies = g
                    .InvolvedCompanies.Where(ic =>
                        !string.IsNullOrEmpty(ic.CompanyNavigation!.Name)
                        && !string.IsNullOrEmpty(ic.CompanyNavigation!.Slug)
                    )
                    .OrderBy(ic => ic.CompanyNavigation!.Name)
                    .Select(ic => new InvolvedCompanyDto(
                        ic.CompanyNavigation!.Name!,
                        ic.CompanyNavigation!.Slug!,
                        ic.Publisher == true,
                        ic.Developer == true
                    ))
                    .ToList(),
                Themes = g
                    .Themes.Where(theme => !string.IsNullOrEmpty(theme.Name))
                    .Select(theme => theme.Name!)
                    .ToList(),
                GameModes = g
                    .GameModes.Where(gameMode => !string.IsNullOrEmpty(gameMode.Name))
                    .Select(gameMode => gameMode.Name!)
                    .ToList(),
                PlayerPerspectives = g
                    .PlayerPerspectives.Where(perspective =>
                        !string.IsNullOrEmpty(perspective.Name)
                    )
                    .Select(perspective => perspective.Name!)
                    .ToList(),
                Websites = g
                    .Websites.Where(website => !string.IsNullOrEmpty(website.Url))
                    .Select(website => new WebsiteDto(
                        website.Url!,
                        website.TypeNavigation!.Type,
                        website.Url!,
                        website.Trusted
                    ))
                    .ToList(),
                AlternativeNames = g
                    .AlternativeNames.Select(alternativeName => new AlternativeNameDto(
                        alternativeName.Id,
                        alternativeName.Name ?? string.Empty,
                        alternativeName.Comment
                    ))
                    .ToList(),
                GameEngines = g
                    .GameEngines.Select(gameEngine => new GameEnginesDto(
                        gameEngine.Id,
                        gameEngine.Name ?? string.Empty,
                        gameEngine.LogoNavigation!.ImageId,
                        gameEngine.Url
                    ))
                    .ToList(),
                LanguageSupports = g
                    .GameLocalizations.Where(localization =>
                        !string.IsNullOrEmpty(localization.Name)
                    )
                    .Select(localization => new LanguageSupportsDto(
                        localization.Name!,
                        null,
                        localization.RegionNavigation!.Name
                    ))
                    .ToList(),
                Franchises = g
                    .Franchises.Where(franchise =>
                        !string.IsNullOrEmpty(franchise.Name)
                        && !string.IsNullOrEmpty(franchise.Slug)
                    )
                    .Select(franchise => new FranchiseDto(franchise.Name!, franchise.Slug!))
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (gameDetailsDto is null)
            return Results.NotFound();

        return Results.Ok(new GetGameResponse(gameDetailsDto));
    }
}
