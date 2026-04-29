using System.ComponentModel.DataAnnotations;
using CatalogAPI.Context;
using CatalogAPI.Data;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace CatalogAPI.Features.MostAnticipated.GetMostAnticipated;

public static class GetMostAnticipatedEndpoint
{
    public sealed record MostAnticipatedResponse(List<MostAnticipatedGame> Games);

    public sealed class MostAnticipatedGame
    {
        [Required]
        public long Id { get; init; }

        [Required]
        public string Name { get; init; } = null!;

        public string? CoverUrl { get; init; }

        [Required]
        public string GameType { get; init; } = null!;

        public int Hypes { get; init; }

        [Required]
        public DateTime FirstReleaseDateUtc { get; init; }
    }

    public static async Task<IResult> HandleAsync(
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var now = Instant.FromDateTimeUtc(DateTime.UtcNow);
        var oneMonthFromNow = now.Plus(Duration.FromDays(30));

        var query = db
            .GamesSearches.AsNoTracking()
            .Where(g => g.FirstReleaseDateUtc.HasValue)
            .Where(g => g.FirstReleaseDateUtc >= now && g.FirstReleaseDateUtc <= oneMonthFromNow)
            .Where(g => g.GameType == "Main Game")
            .OrderByDescending(g =>
                (g.SteamMostWishlistedUpcoming ?? 0) * 0.5
                + (g.Hypes ?? 0) * 0.3
                + (g.IgdbVisits ?? 0) * 0.2
            )
            .Take(6)
            .Select(g => new MostAnticipatedGame
            {
                Id = g.Id,
                Name = g.Name!,
                CoverUrl = g.CoverUrl,
                GameType = g.GameType!,
                Hypes = g.Hypes ?? 0,
                FirstReleaseDateUtc = g.FirstReleaseDateUtc!.Value.ToDateTimeUtc(),
            });

        var games = await query.ToListAsync(cancellationToken);

        return Results.Ok(new MostAnticipatedResponse(games));
    }
}
