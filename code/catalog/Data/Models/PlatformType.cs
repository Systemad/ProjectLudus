using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// platform_types lookup table.
/// </summary>
public partial class PlatformType
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();
}
