namespace CatalogAPI.Features.Games.GetGamePageReleaseData;

public static class GetGamePageReleaseDataEndpoint
{
    public record GetGamePageReleaseDataResponse(GamePageReleaseDataDto Data);

    public static async Task<IResult> HandleAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var releaseData = await db
            .Games.Where(g => g.Id == gameId)
            .Select(g => new GamePageReleaseDataDto
            {
                GameId = g.Id,
                GameName = g.Name ?? string.Empty,
                Releases = g
                    .ReleaseDates.Where(rd => rd.PlatformNavigation != null)
                    .Select(rd => new GameReleaseDto
                    {
                        PlatformName = rd.PlatformNavigation!.Name,
                        PlatformSlug = rd.PlatformNavigation!.Slug,
                        ReleaseDate = rd.Date,
                        Region = rd.ReleaseRegionNavigation!.Region,
                        Human = rd.Human,
                        Developers = g
                            .InvolvedCompanies.Where(ic =>
                                ic.Developer == true && ic.CompanyNavigation!.Name != null
                            )
                            .Select(ic => new CompanyInfoDto(
                                ic.CompanyNavigation!.Id,
                                ic.CompanyNavigation!.Name!,
                                ic.CompanyNavigation.Slug
                            ))
                            .ToList(),
                        Publishers = g
                            .InvolvedCompanies.Where(ic =>
                                ic.Publisher == true && ic.CompanyNavigation!.Name != null
                            )
                            .Select(ic => new CompanyInfoDto(
                                ic.CompanyNavigation!.Id,
                                ic.CompanyNavigation!.Name!,
                                ic.CompanyNavigation.Slug
                            ))
                            .ToList(),
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (releaseData is null)
            return Results.NotFound();

        return Results.Ok(new GetGamePageReleaseDataResponse(releaseData));
    }
}
