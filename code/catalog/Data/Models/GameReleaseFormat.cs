using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// game_release_formats lookup table.
/// </summary>
public partial class GameReleaseFormat
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Format { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<ExternalGame> ExternalGames { get; set; } = new List<ExternalGame>();
}
