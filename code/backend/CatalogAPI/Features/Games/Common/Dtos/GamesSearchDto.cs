using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Games.Common.Dtos;

public partial class GamesSearchDto
{
    [Required]
    public required long Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Summary { get; set; }

    public string? Storyline { get; set; }

    public long? UpdatedAt { get; set; }

    public long? FirstReleaseDate { get; set; }

    public string? GameType { get; set; }

    public string? GameStatus { get; set; }

    public double? AggregatedRating { get; set; }

    public int? AggregatedRatingCount { get; set; }

    public double? Rating { get; set; }

    public int? RatingCount { get; set; }

    public double? TotalRating { get; set; }

    public int? TotalRatingCount { get; set; }

    public string? CoverUrl { get; set; }

    [Required]
    public required List<string> Themes { get; set; } = [];

    [Required]
    public required List<string> Genres { get; set; } = [];

    [Required]
    public required List<string> GameModes { get; set; } = [];

    [Required]
    public required List<string> Platforms { get; set; } = [];

    [Required]
    public required List<string> GameEngines { get; set; } = [];

    [Required]
    public required List<string> PlayerPerspectives { get; set; } = [];

    [Required]
    public required List<string> Publishers { get; set; } = [];

    [Required]
    public required List<string> Developers { get; set; } = [];

    [Required]
    public required List<string> MultiplayerModes { get; set; } = [];
    public long? TotalVisits { get; set; }

    public double? IgdbVisits { get; set; }

    public double? IgdbWantToPlay { get; set; }

    public double? IgdbPlaying { get; set; }

    public double? IgdbPlayed { get; set; }

    public double? Steam24hrPeakPlayers { get; set; }

    public double? SteamPositiveReviews { get; set; }

    public double? SteamNegativeReviews { get; set; }

    public double? SteamTotalReviews { get; set; }

    public double? SteamGlobalTopSellers { get; set; }

    public double? SteamMostWishlistedUpcoming { get; set; }

    public double? Twitch24hrHoursWatched { get; set; }

    public int? ReleaseYear { get; set; }
}
