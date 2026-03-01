using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Game
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public long? ParentGame { get; set; }

    public string? Slug { get; set; }

    public string? Summary { get; set; }

    public long? Cover { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public long? GameType { get; set; }

    public long? FirstReleaseDate { get; set; }

    public double? Rating { get; set; }

    public long? RatingCount { get; set; }

    public double? TotalRating { get; set; }

    public long? TotalRatingCount { get; set; }

    public string? Storyline { get; set; }

    public double? AggregatedRating { get; set; }

    public long? AggregatedRatingCount { get; set; }

    public long? Status { get; set; }

    public long? GameStatus { get; set; }

    public long? Hypes { get; set; }

    public long? VersionParent { get; set; }

    public string? VersionTitle { get; set; }

    public long? Franchise { get; set; }

    public virtual ICollection<AlternativeName> AlternativeNames { get; set; } = new List<AlternativeName>();

    public virtual Cover? CoverNavigation { get; set; }

    public virtual ICollection<ExternalGame> ExternalGames { get; set; } = new List<ExternalGame>();

    public virtual Franchise? FranchiseNavigation { get; set; }

    public virtual ICollection<GameLocalization> GameLocalizations { get; set; } = new List<GameLocalization>();

    public virtual GameStatus? GameStatusNavigation { get; set; }

    public virtual GameType? GameTypeNavigation { get; set; }

    public virtual ICollection<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();

    public virtual ICollection<Screenshot> Screenshots { get; set; } = new List<Screenshot>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

    public virtual ICollection<Website> Websites { get; set; } = new List<Website>();

    public virtual ICollection<Game> BaseGames { get; set; } = new List<Game>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Company> CompaniesNavigation { get; set; } = new List<Company>();

    public virtual ICollection<Game> DlcGames { get; set; } = new List<Game>();

    public virtual ICollection<Franchise> Franchises { get; set; } = new List<Franchise>();

    public virtual ICollection<GameMode> GameModes { get; set; } = new List<GameMode>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Keyword> Keywords { get; set; } = new List<Keyword>();

    public virtual ICollection<PlayerPerspective> PlayerPerspectives { get; set; } = new List<PlayerPerspective>();

    public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();
}
