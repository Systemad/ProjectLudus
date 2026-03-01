using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class PlatformFamily
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
