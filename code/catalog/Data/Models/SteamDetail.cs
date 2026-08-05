using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class SteamDetail
{
    public long GameId { get; set; }

    public long? SteamAppId { get; set; }

    public string? HeaderUrl { get; set; }

    public string? CapsuleUrl { get; set; }

    public virtual Game Game { get; set; } = null!;
}
