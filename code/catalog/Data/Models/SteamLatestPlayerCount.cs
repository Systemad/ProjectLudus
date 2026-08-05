using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class SteamLatestPlayerCount
{
    public long GameId { get; set; }

    public long? SteamAppId { get; set; }

    public long? CurrentPlayers { get; set; }

    public DateTime? CapturedAt { get; set; }

    public long? Peak24h { get; set; }

    public List<int>? Sparkline7d { get; set; }

    public virtual Game Game { get; set; } = null!;
}
