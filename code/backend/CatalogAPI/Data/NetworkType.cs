using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class NetworkType
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string EventNetworks { get; set; } = null!;

    public string? Checksum { get; set; }

    public virtual ICollection<EventNetwork1> EventNetwork1s { get; set; } = new List<EventNetwork1>();
}
