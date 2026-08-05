using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class PopularityPrimitive
{
    public long GameId { get; set; }

    public long PopularityType { get; set; }

    public double? Value { get; set; }

    public long? CalculatedAt { get; set; }

    public DateTime? CapturedAt { get; set; }

    public virtual Game Game { get; set; } = null!;
}
