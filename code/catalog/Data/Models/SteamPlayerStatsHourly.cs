using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class SteamPlayerStatsHourly
{
    public long? GameId { get; set; }

    public DateTime? Bucket { get; set; }

    public int? PeakPlayers { get; set; }

    public int? AvgPlayers { get; set; }
}
