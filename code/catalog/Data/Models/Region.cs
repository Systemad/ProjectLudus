using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// regions lookup table.
/// </summary>
public partial class Region
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Identifier { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<GameLocalization> GameLocalizations { get; set; } = new List<GameLocalization>();
}
