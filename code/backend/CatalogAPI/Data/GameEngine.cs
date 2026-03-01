using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class GameEngine
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public string? Description { get; set; }

    public long? Logo { get; set; }

    public virtual GameEngineLogo? LogoNavigation { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
