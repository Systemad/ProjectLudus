using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their expansions.
/// </summary>
public partial class GameExpansion
{
    public long? ExpansionSourceId { get; set; }

    public long ExpansionId { get; set; }

    public virtual Game Expansion { get; set; } = null!;
}
