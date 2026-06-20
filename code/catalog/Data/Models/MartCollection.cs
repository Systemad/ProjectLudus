using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Game collections (series, bundles, etc.).
/// </summary>
public partial class MartCollection
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    /// <summary>
    /// Collection name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Collection slug.
    /// </summary>
    public string Slug { get; set; } = null!;

    /// <summary>
    /// Collection URL.
    /// </summary>
    public string Url { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    /// <summary>
    /// FK to mart_collection_types.id.
    /// </summary>
    public long? Type { get; set; }

    public virtual ICollection<MartCollectionMembership> MartCollectionMemberships { get; set; } = new List<MartCollectionMembership>();

    public virtual ICollection<MartCollectionRelation> MartCollectionRelationChildCollectionNavigations { get; set; } = new List<MartCollectionRelation>();

    public virtual ICollection<MartCollectionRelation> MartCollectionRelationParentCollectionNavigations { get; set; } = new List<MartCollectionRelation>();
}
