using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Game
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public long? ParentGame { get; set; }

    public string Slug { get; set; } = null!;

    public string? Summary { get; set; }

    public long? Cover { get; set; }

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public long? GameType { get; set; }

    public long? FirstReleaseDateEpoch { get; set; }

    public DateTime? FirstReleaseDateUtc { get; set; }

    public double? Rating { get; set; }

    public long? RatingCount { get; set; }

    public double? TotalRating { get; set; }

    public long? TotalRatingCount { get; set; }

    public string? Storyline { get; set; }

    public double? AggregatedRating { get; set; }

    public long? AggregatedRatingCount { get; set; }

    public long? GameStatus { get; set; }

    public long? Hypes { get; set; }

    public long? VersionParent { get; set; }

    public string? VersionTitle { get; set; }

    public long? Franchise { get; set; }

    public virtual ICollection<AlternativeName> AlternativeNames { get; set; } = new List<AlternativeName>();

    public virtual ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();

    public virtual ICollection<CollectionMembership> CollectionMemberships { get; set; } = new List<CollectionMembership>();

    public virtual Cover? CoverNavigation { get; set; }

    public virtual ICollection<ExternalGame> ExternalGames { get; set; } = new List<ExternalGame>();

    public virtual Franchise? FranchiseNavigation { get; set; }

    public virtual GameExpandedGame? GameExpandedGame { get; set; }

    public virtual GameExpansion? GameExpansion { get; set; }

    public virtual GameFork? GameFork { get; set; }

    public virtual ICollection<GameLocalization> GameLocalizations { get; set; } = new List<GameLocalization>();

    public virtual GamePort? GamePort { get; set; }

    public virtual GameRemake? GameRemake { get; set; }

    public virtual GameRemaster? GameRemaster { get; set; }

    public virtual GameStandaloneExpansion? GameStandaloneExpansion { get; set; }

    public virtual GameStatus? GameStatusNavigation { get; set; }

    public virtual ICollection<GameTimeToBeat> GameTimeToBeats { get; set; } = new List<GameTimeToBeat>();

    public virtual GameType? GameTypeNavigation { get; set; }

    public virtual GamesDlc? GamesDlc { get; set; }

    public virtual ICollection<InvolvedCompany> InvolvedCompanies { get; set; } = new List<InvolvedCompany>();

    public virtual ICollection<LanguageSupport> LanguageSupports { get; set; } = new List<LanguageSupport>();

    public virtual ICollection<MultiplayerMode> MultiplayerModesNavigation { get; set; } = new List<MultiplayerMode>();

    public virtual ICollection<PopularityPrimitive> PopularityPrimitives { get; set; } = new List<PopularityPrimitive>();

    public virtual ICollection<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();

    public virtual ICollection<Screenshot> Screenshots { get; set; } = new List<Screenshot>();

    public virtual SteamDetail? SteamDetail { get; set; }

    public virtual SteamLatestPlayerCount? SteamLatestPlayerCount { get; set; }

    public virtual SteamLatestPricing? SteamLatestPricing { get; set; }

    public virtual SteamReview? SteamReview { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

    public virtual ICollection<Website> Websites { get; set; } = new List<Website>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Franchise> Franchises { get; set; } = new List<Franchise>();

    public virtual ICollection<GameEngine> GameEngines { get; set; } = new List<GameEngine>();

    public virtual ICollection<GameMode> GameModes { get; set; } = new List<GameMode>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Keyword> Keywords { get; set; } = new List<Keyword>();

    public virtual ICollection<MultiplayerMode> MultiplayerModes { get; set; } = new List<MultiplayerMode>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();

    public virtual ICollection<PlayerPerspective> PlayerPerspectives { get; set; } = new List<PlayerPerspective>();

    public virtual ICollection<Game> SimilarGames { get; set; } = new List<Game>();

    public virtual ICollection<Game> SimilarSources { get; set; } = new List<Game>();

    public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();
}
