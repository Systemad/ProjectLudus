using System.Collections.Generic;
using System.Linq;
using CatalogAPI.Data;

namespace CatalogAPI.Dtos;

public sealed record GameDto
{
    public long Id { get; init; }
    public long? CreatedAt { get; init; }
    public long? UpdatedAt { get; init; }
    public string? Name { get; init; }
    public string? Summary { get; init; }
    public string? Storyline { get; init; }
    public string? Slug { get; init; }
    public string? Url { get; init; }
    public string? Checksum { get; init; }
    public long? ParentGame { get; init; }
    public long? Cover { get; init; }
    public string? CoverUrl { get; init; }
    public long? GameType { get; init; }
    public string? GameTypeName { get; init; }
    public long? GameStatus { get; init; }
    public string? GameStatusName { get; init; }
    public long? FirstReleaseDate { get; init; }
    public double? Rating { get; init; }
    public long? RatingCount { get; init; }
    public double? TotalRating { get; init; }
    public long? TotalRatingCount { get; init; }
    public double? AggregatedRating { get; init; }
    public long? AggregatedRatingCount { get; init; }
    public long? Hypes { get; init; }
    public long? VersionParent { get; init; }
    public string? VersionTitle { get; init; }
    public long? Franchise { get; init; }
    public IReadOnlyList<string>? Platforms { get; init; }
    public IReadOnlyList<string>? Genres { get; init; }
    public IReadOnlyList<string>? Themes { get; init; }
    public IReadOnlyList<string>? GameModes { get; init; }
    public IReadOnlyList<string>? PlayerPerspectives { get; init; }
    public IReadOnlyList<ReleaseDateDto>? ReleaseDates { get; init; }
    public IReadOnlyList<WebsiteDto>? Websites { get; init; }
    public IReadOnlyList<VideoDto>? Videos { get; init; }
    public IReadOnlyList<ScreenshotDto>? Screenshots { get; init; }
    public IReadOnlyList<AlternativeNameDto>? AlternativeNames { get; init; }
    public IReadOnlyList<ExternalGameDto>? ExternalGames { get; init; }

    public static GameDto From(Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            CreatedAt = game.CreatedAt,
            UpdatedAt = game.UpdatedAt,
            Name = game.Name,
            Summary = game.Summary,
            Storyline = game.Storyline,
            Slug = game.Slug,
            Url = game.Url,
            Checksum = game.Checksum,
            ParentGame = game.ParentGame,
            Cover = game.Cover,
            CoverUrl = game.CoverNavigation?.Url,
            GameType = game.GameType,
            GameTypeName = game.GameTypeNavigation?.Type,
            GameStatus = game.GameStatus,
            GameStatusName = game.GameStatusNavigation?.Status,
            FirstReleaseDate = game.FirstReleaseDate,
            Rating = game.Rating,
            RatingCount = game.RatingCount,
            TotalRating = game.TotalRating,
            TotalRatingCount = game.TotalRatingCount,
            AggregatedRating = game.AggregatedRating,
            AggregatedRatingCount = game.AggregatedRatingCount,
            Hypes = game.Hypes,
            VersionParent = game.VersionParent,
            VersionTitle = game.VersionTitle,
            Franchise = game.Franchise,
            Platforms = game
                .Platforms?.Select(p => p.Name ?? string.Empty)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList(),
            Genres = game
                .Genres?.Select(g => g.Name ?? string.Empty)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList(),
            Themes = game
                .Themes?.Select(t => t.Name ?? string.Empty)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList(),
            GameModes = game
                .GameModes?.Select(m => m.Name ?? string.Empty)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList(),
            PlayerPerspectives = game
                .PlayerPerspectives?.Select(p => p.Name ?? string.Empty)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList(),
            ReleaseDates = game.ReleaseDates?.Select(ReleaseDateDto.From).ToList(),
            Websites = game.Websites?.Select(WebsiteDto.From).ToList(),
            Videos = game.Videos?.Select(VideoDto.From).ToList(),
            Screenshots = game.Screenshots?.Select(ScreenshotDto.From).ToList(),
            AlternativeNames = game.AlternativeNames?.Select(AlternativeNameDto.From).ToList(),
            ExternalGames = game.ExternalGames?.Select(ExternalGameDto.From).ToList(),
        };
    }
}

public sealed record ReleaseDateDto
{
    public long Id { get; init; }
    public long? Date { get; init; }
    public string? Human { get; init; }
    public long? Platform { get; init; }
    public string? PlatformName { get; init; }
    public long? Status { get; init; }
    public string? StatusName { get; init; }
    public long? ReleaseRegion { get; init; }
    public string? ReleaseRegionName { get; init; }
    public long? DateFormat { get; init; }
    public string? DateFormatName { get; init; }
    public long? Y { get; init; }
    public long? M { get; init; }

    public static ReleaseDateDto From(ReleaseDate releaseDate)
    {
        return new ReleaseDateDto
        {
            Id = releaseDate.Id,
            Date = releaseDate.Date,
            Human = releaseDate.Human,
            Platform = releaseDate.Platform,
            PlatformName = releaseDate.PlatformNavigation?.Name,
            Status = releaseDate.Status,
            StatusName = releaseDate.StatusNavigation?.Name,
            ReleaseRegion = releaseDate.ReleaseRegion,
            ReleaseRegionName = releaseDate.ReleaseRegionNavigation?.Region,
            DateFormat = releaseDate.DateFormat,
            DateFormatName = releaseDate.DateFormatNavigation?.Format,
            Y = releaseDate.Y,
            M = releaseDate.M,
        };
    }
}

public sealed record WebsiteDto(long Id, bool? Trusted, string? Url, string? TypeName)
{
    public static WebsiteDto From(Website website) =>
        new(website.Id, website.Trusted, website.Url, website.TypeNavigation?.Type);
}

public sealed record VideoDto(long Id, string? Name, string? VideoId)
{
    public static VideoDto From(Video video) => new(video.Id, video.Name, video.VideoId);
}

public sealed record ScreenshotDto(
    long Id,
    bool? AlphaChannel,
    bool? Animated,
    long? Height,
    long? Width,
    string? Url
)
{
    public static ScreenshotDto From(Screenshot screenshot) =>
        new(
            screenshot.Id,
            screenshot.AlphaChannel,
            screenshot.Animated,
            screenshot.Height,
            screenshot.Width,
            screenshot.Url
        );
}

public sealed record AlternativeNameDto(long Id, string? Name, string? Comment)
{
    public static AlternativeNameDto From(AlternativeName alternativeName) =>
        new(alternativeName.Id, alternativeName.Name, alternativeName.Comment);
}

public sealed record ExternalGameDto(
    long Id,
    string? Name,
    string? Uid,
    string? Url,
    long? PlatformId,
    string? PlatformName,
    string? ExternalGameSourceName
)
{
    public static ExternalGameDto From(ExternalGame externalGame) =>
        new(
            externalGame.Id,
            externalGame.Name,
            externalGame.Uid,
            externalGame.Url,
            externalGame.Platform,
            externalGame.PlatformNavigation?.Name,
            externalGame.ExternalGameSourceNavigation?.Name
        );
}
