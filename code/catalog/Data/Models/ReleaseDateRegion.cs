using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// release_date_regions lookup table.
/// </summary>
public partial class ReleaseDateRegion
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Region { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();
}
