using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GameEngine
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public string? Description { get; set; }

    public long? Logo { get; set; }

    public virtual GameEngineLogo? LogoNavigation { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
