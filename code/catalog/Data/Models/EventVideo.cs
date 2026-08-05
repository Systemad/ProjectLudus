using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class EventVideo
{
    public long EventId { get; set; }

    public long VideoId { get; set; }

    public virtual Event Event { get; set; } = null!;
}
