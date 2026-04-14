using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

/// <summary>
/// Search-ready games dataset with aggregated metrics for Typesense indexing
/// </summary>
public partial class GamesSearch
{
    public long Id { get; set; }

    public string? Name { get; set; }

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

    public List<string>? Themes { get; set; }

    public List<string>? Genres { get; set; }

    public List<string>? GameModes { get; set; }

    public List<string>? Platforms { get; set; }

    public List<string>? GameEngines { get; set; }

    public List<string>? PlayerPerspectives { get; set; }

    public List<string>? Publishers { get; set; }

    public List<string>? Developers { get; set; }

    public List<string>? MultiplayerModes { get; set; }

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
