using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class SteamReview
{
    public long GameId { get; set; }

    public long? SteamAppId { get; set; }

    public int? NumReviews { get; set; }

    public int? ReviewScore { get; set; }

    public string? ReviewScoreDesc { get; set; }

    public int? TotalPositive { get; set; }

    public int? TotalNegative { get; set; }

    public int? TotalReviews { get; set; }

    public virtual Game Game { get; set; } = null!;
}
