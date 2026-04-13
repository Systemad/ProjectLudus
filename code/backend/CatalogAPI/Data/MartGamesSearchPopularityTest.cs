namespace CatalogAPI.Data;

public partial class MartGamesSearchPopularityTest
{
    public long? Id { get; set; }

    public string? Name { get; set; }

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
}
