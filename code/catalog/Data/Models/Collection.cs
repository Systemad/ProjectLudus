using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Game collections (series, bundles, etc.).
/// </summary>
public partial class Collection
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

    public virtual ICollection<CollectionMembership> CollectionMemberships { get; set; } = new List<CollectionMembership>();

    public virtual ICollection<CollectionRelation> CollectionRelationChildCollectionNavigations { get; set; } = new List<CollectionRelation>();

    public virtual ICollection<CollectionRelation> CollectionRelationParentCollectionNavigations { get; set; } = new List<CollectionRelation>();

    public virtual CollectionType? TypeNavigation { get; set; }
}
