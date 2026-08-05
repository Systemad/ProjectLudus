using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class TrackedGame
{
    public long GameId { get; set; }

    public long SteamAppId { get; set; }

    public DateTime RefreshedAt { get; set; }
}
