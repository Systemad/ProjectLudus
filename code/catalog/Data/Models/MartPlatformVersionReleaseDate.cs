using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Release dates for specific platform versions.
/// </summary>
public partial class MartPlatformVersionReleaseDate
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    /// <summary>
    /// Release date (Unix timestamp).
    /// </summary>
    public long? Date { get; set; }

    /// <summary>
    /// Human-readable release date.
    /// </summary>
    public string? Human { get; set; }

    /// <summary>
    /// Month.
    /// </summary>
    public long? M { get; set; }

    /// <summary>
    /// Year.
    /// </summary>
    public long? Y { get; set; }

    public string Checksum { get; set; } = null!;

    /// <summary>
    /// FK to date_formats.id.
    /// </summary>
    public long? DateFormat { get; set; }

    /// <summary>
    /// FK to release_date_regions.id.
    /// </summary>
    public long? ReleaseRegion { get; set; }
}
