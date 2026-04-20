using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class PopularityType
{
    public long Id { get; set; }

    public long? PopularitySource { get; set; }

    public string? Name { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Checksum { get; set; }

    public long? ExternalPopularitySource { get; set; }

    public virtual ICollection<GamePopularityLatest> GamePopularityLatests { get; set; } = new List<GamePopularityLatest>();

    public virtual ICollection<PopularityPrimitive> PopularityPrimitives { get; set; } = new List<PopularityPrimitive>();
}
