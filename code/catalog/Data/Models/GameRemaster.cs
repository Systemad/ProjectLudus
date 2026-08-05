using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their remasters.
/// </summary>
public partial class GameRemaster
{
    public long? RemasterSourceId { get; set; }

    public long RemasterId { get; set; }

    public virtual Game Remaster { get; set; } = null!;
}
