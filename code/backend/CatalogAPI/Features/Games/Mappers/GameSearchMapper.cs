namespace CatalogAPI.Features.Games.Mappers;

public static partial class GameSearchMapper
{
    public static List<GamesSearchDto> MapToDto(IEnumerable<GamesSearch>? q)
    {
        return q?.Select(MapToDto).ToList() ?? [];
    }

    private static GamesSearchDto MapToDto(GamesSearch source)
    {
        return new GamesSearchDto
        {
            Id = source.Id,
            Name = source.Name ?? string.Empty,
            Summary = source.Summary,
            Storyline = source.Storyline,
            UpdatedAt = source.UpdatedAt,
            FirstReleaseDateEpoch = source.FirstReleaseDateEpoch,
            FirstReleaseDate = MapFirstReleaseDate(source),
            GameType = source.GameType,
            GameStatus = source.GameStatus,
            AggregatedRating = source.AggregatedRating,
            AggregatedRatingCount = source.AggregatedRatingCount,
            Rating = source.Rating,
            RatingCount = source.RatingCount,
            TotalRating = source.TotalRating,
            TotalRatingCount = source.TotalRatingCount,
            CoverUrl = source.CoverUrl,
            Themes = source.Themes,
            Genres = source.Genres,
            GameModes = source.GameModes,
            Platforms = source.Platforms,
            GameEngines = source.GameEngines,
            PlayerPerspectives = source.PlayerPerspectives,
            Publishers = source.Publishers,
            Developers = source.Developers,
            MultiplayerModes = source.MultiplayerModes,
            TotalVisits = source.TotalVisits,
            IgdbVisits = source.IgdbVisits,
            IgdbWantToPlay = source.IgdbWantToPlay,
            IgdbPlaying = source.IgdbPlaying,
            IgdbPlayed = source.IgdbPlayed,
            Steam24hrPeakPlayers = source.Steam24hrPeakPlayers,
            SteamPositiveReviews = source.SteamPositiveReviews,
            SteamNegativeReviews = source.SteamNegativeReviews,
            SteamTotalReviews = source.SteamTotalReviews,
            SteamGlobalTopSellers = source.SteamGlobalTopSellers,
            SteamMostWishlistedUpcoming = source.SteamMostWishlistedUpcoming,
            Twitch24hrHoursWatched = source.Twitch24hrHoursWatched,
            ReleaseYear = source.ReleaseYear,
        };
    }

    private static DateOnly MapFirstReleaseDate(GamesSearch source)
    {
        if (source.FirstReleaseDateUtc is Instant firstReleaseDateUtc)
        {
            return DateOnly.FromDateTime(firstReleaseDateUtc.ToDateTimeUtc());
        }

        return DateOnly.MinValue;
    }
}
