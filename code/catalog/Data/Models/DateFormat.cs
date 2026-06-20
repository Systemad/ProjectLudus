using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// date_formats lookup table.
/// </summary>
public partial class DateFormat
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Format { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<ReleaseDate> ReleaseDates { get; set; } = new List<ReleaseDate>();
}
