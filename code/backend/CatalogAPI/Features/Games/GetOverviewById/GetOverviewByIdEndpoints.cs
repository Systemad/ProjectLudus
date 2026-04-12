using CatalogAPI.Context;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Games.GetOverviewById;

public static class GetOverviewByIdEndpoints
{
    public record GetGameOverviewByIdResponse(GameOverviewDto Game);

    public static IEndpointRouteBuilder MapGetOverviewByIdEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games").CacheOutput("DefaultCache");

        group
            .MapGet("/{gameId:long}", GetGameOverviewByIdAsync)
            .Produces<GetGameOverviewByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return routeBuilder;
    }

    private static async Task<IResult> GetGameOverviewByIdAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var nowUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var gameOverview = await db
            .Games.AsNoTracking()
            .Where(g => g.Id == gameId)
            .Select(g => new GameOverviewDto
            {
                Id = g.Id,
                Slug = g.Slug,
                Name = g.Name,
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
                    .Select(p => new PlatformsDto(p.Name, p.Slug))
                    .ToList(),
                IsReleased = g!.FirstReleaseDate <= nowUnixSeconds,
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

        return Results.Ok(new GetGameOverviewByIdResponse(gameOverview));
    }
}
