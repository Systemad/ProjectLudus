using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their expanded games.
/// </summary>
public partial class GameExpandedGame
{
    public long? ExpandedSourceId { get; set; }

    public long ExpandedGameId { get; set; }

    public virtual Game ExpandedGame { get; set; } = null!;
}
