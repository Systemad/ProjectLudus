using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// player_perspectives lookup table.
/// </summary>
public partial class PlayerPerspective
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
