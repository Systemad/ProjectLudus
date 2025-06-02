namespace Ludus.Server.Features.Games;

public record GameDto(
    long Id,
    string Name,
    string ArtworkImageId,
    string CoverImageId,
    long FirstReleaseDate,
    string Publisher,
    List<string> Platforms,
    List<DateTime> ReleaseDates,
    string GameType,
    bool IsWishlisted = false,
    bool IsFavorited = false
);
