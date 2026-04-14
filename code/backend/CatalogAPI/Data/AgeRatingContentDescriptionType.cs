using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class AgeRatingContentDescriptionType
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Slug { get; set; }

    public string? Name { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<AgeRatingContentDescriptionsV2> AgeRatingContentDescriptionsV2s { get; set; } = new List<AgeRatingContentDescriptionsV2>();
}
