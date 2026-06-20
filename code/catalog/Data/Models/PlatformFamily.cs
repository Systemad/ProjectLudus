using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// platform_family lookup table.
/// </summary>
public partial class PlatformFamily
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
