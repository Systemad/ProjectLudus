using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Platform
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? AlternativeName { get; set; }

    public long? Generation { get; set; }

    public string? Name { get; set; }

    public long? PlatformLogo { get; set; }

    public string? Slug { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public long? PlatformType { get; set; }

    public long? PlatformFamily { get; set; }

    public string? Abbreviation { get; set; }

    public string? Summary { get; set; }

    public virtual ICollection<ExternalGame> ExternalGames { get; set; } = new List<ExternalGame>();

    public virtual PlatformFamily? PlatformFamilyNavigation { get; set; }

    public virtual PlatformLogo? PlatformLogoNavigation { get; set; }

    public virtual PlatformType? PlatformTypeNavigation { get; set; }

    public virtual ICollection<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();

    public virtual ICollection<GameEngine> GameEngines { get; set; } = new List<GameEngine>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
