using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their remakes.
/// </summary>
public partial class GameRemake
{
    public long? RemakeSourceId { get; set; }

    public long RemakeId { get; set; }

    public virtual Game Remake { get; set; } = null!;
}
