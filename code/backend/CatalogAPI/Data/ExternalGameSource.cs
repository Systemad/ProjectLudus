using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class ExternalGameSource
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<ExternalGame> ExternalGames { get; set; } = new List<ExternalGame>();

    public virtual ICollection<PopularityPrimitive> PopularityPrimitives { get; set; } = new List<PopularityPrimitive>();
}
