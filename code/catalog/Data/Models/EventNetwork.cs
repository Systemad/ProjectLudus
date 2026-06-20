using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class EventNetwork
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public long? Event { get; set; }

    public string Url { get; set; } = null!;

    public long? NetworkType { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual Event? EventNavigation { get; set; }

    public virtual NetworkType? NetworkTypeNavigation { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
