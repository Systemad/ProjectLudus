namespace CatalogAPI.Features.Games.GetOverview;

public static class GetGameOverviewEndpoint
{
    public record GetGameOverviewResponse(GameOverviewDto Game);

    public static async Task<IResult> HandleAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var gameOverview = await db
            .Games.Where(g => g.Id == gameId)
            .Select(g => new GameOverviewDto
            {
                Id = g.Id,
                Slug = g.Slug,
                Name = g.Name ?? string.Empty,
                Summary = g.Summary,
                Storyline = g.Storyline,
                Cover = g.CoverNavigation!.ImageId,
                CoverUrl = g.CoverNavigation!.Url,
                GameType = g.GameType,
                GameTypeName = g.GameTypeNavigation!.Type,
                Genres = g
                    .Genres.Where(genre => !string.IsNullOrEmpty(genre.Name))
                    .Select(genre => genre.Name!)
                    .ToList(),
                Themes = g
                    .Themes.Where(theme => !string.IsNullOrEmpty(theme.Name))
                    .Select(theme => theme.Name!)
                    .ToList(),
                Platforms = g
                    .Platforms.Where(platform => !string.IsNullOrEmpty(platform.Name))
                    .Select(p => new PlatformsDto(p.Name!, p.Slug))
                    .ToList(),
                IsReleased = g!.FirstReleaseDateUtc <= SystemClock.Instance.GetCurrentInstant(),
                ReleaseDatePlatform = g
                    .ReleaseDates.Select(releaseDate => new ReleaseDatePlatformDto(
                        releaseDate.Date,
                        releaseDate.PlatformNavigation!.Name
                    ))
                    .ToList(),
                ReleaseDates = g
                    .ReleaseDates.Select(releaseDate => new ReleaseDatesDto(
                        releaseDate.Date,
                        releaseDate.ReleaseRegionNavigation!.Region
                    ))
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (gameOverview is null)
            return Results.NotFound();

        return Results.Ok(new GetGameOverviewResponse(gameOverview));
    }
}
