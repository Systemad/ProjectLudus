using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class EventNetwork
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public long? Event { get; set; }

    public string? Url { get; set; }

    public long? NetworkType { get; set; }

    public string? Checksum { get; set; }

    public virtual Event? EventNavigation { get; set; }

    public virtual NetworkType? NetworkTypeNavigation { get; set; }
}
