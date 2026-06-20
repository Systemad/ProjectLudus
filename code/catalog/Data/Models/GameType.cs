using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// game_types lookup table.
/// </summary>
public partial class GameType
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Type { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
