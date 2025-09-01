using Shared.Features.Games;

namespace Shared.Features;

public class IgdbGameDto
{
    public long Id { get; set; }

    public List<AgeRating> AgeRatings { get; set; } = new();

    public List<AlternativeName> AlternativeNames { get; set; } = new();

    public List<Artwork> Artworks { get; set; } = new();

    public Cover? Cover { get; set; }

    public required long CreatedAt { get; set; }

    public List<long> Dlcs { get; set; } = new();

    public List<long> Expansions { get; set; } = new();

    public long FirstReleaseDate { get; set; }

    public List<Franchise> Franchises { get; set; } = new();

    public List<GameEngine> GameEngines { get; set; } = new();

    public List<GameMode> GameModes { get; set; } = new();

    public List<Genre> Genres { get; set; } = new();

    public long? Hypes { get; set; }

    public List<InvolvedCompany> InvolvedCompanies { get; set; } = new();

    public List<Keyword> Keywords { get; set; } = new();

    public required string Name { get; set; }

    public List<Platform> Platforms { get; set; } = new();

    public List<PlayerPerspective> PlayerPerspectives { get; set; } = new();

    public double? Rating { get; set; }

    public long? RatingCount { get; set; }

    public List<ReleaseDate> ReleaseDates { get; set; } = new();

    public List<Screenshot> Screenshots { get; set; } = new();

    public List<long> SimilarGames { get; set; } = new();

    public string? Slug { get; set; }

    public string? Storyline { get; set; }

    public string? Summary { get; set; }

    public List<Theme> Themes { get; set; } = new();

    public double? TotalRating { get; set; }

    public long? TotalRatingCount { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Url { get; set; }

    public List<Video> Videos { get; set; } = new();

    public List<GameWebsite> Websites { get; set; } = new();

    public required string Checksum { get; set; }

    public List<LanguageSupport> LanguageSupports { get; set; } = new();

    public List<Collection> Collections { get; set; } = new();

    public GameType GameType { get; set; } = new();

    public List<MultiplayerMode> MultiplayerModes { get; set; } = new();
}