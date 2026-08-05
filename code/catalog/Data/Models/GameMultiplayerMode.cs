using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GameMultiplayerMode
{
    public long GameId { get; set; }

    public long MultiplayerModeId { get; set; }

    public virtual Game Game { get; set; } = null!;
}
