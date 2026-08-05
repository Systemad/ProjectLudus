using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class SteamLatestPricing
{
    public long GameId { get; set; }

    public long? SteamAppId { get; set; }

    public int? InitialCents { get; set; }

    public int? FinalCents { get; set; }

    public int? DiscountPercent { get; set; }

    public string? Currency { get; set; }

    public string? InitialFormatted { get; set; }

    public string? FinalFormatted { get; set; }

    public int? High30d { get; set; }

    public int? Low30d { get; set; }

    public DateTime? CapturedAt { get; set; }

    public virtual Game Game { get; set; } = null!;
}
