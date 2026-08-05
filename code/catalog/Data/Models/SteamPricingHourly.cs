using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class SteamPricingHourly
{
    public long? GameId { get; set; }

    public DateTime? Bucket { get; set; }

    public int? MinPrice { get; set; }

    public int? MaxPrice { get; set; }

    public int? AvgPrice { get; set; }
}
