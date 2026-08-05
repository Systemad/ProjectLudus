using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GameGameMode
{
    public long GameId { get; set; }

    public long GameModeId { get; set; }

    public virtual Game Game { get; set; } = null!;
}
