using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Hierarchy links between collections (child-parent).
/// </summary>
public partial class CollectionRelation
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// FK to mart_collections.id.
    /// </summary>
    public long? ChildCollection { get; set; }

    /// <summary>
    /// FK to mart_collections.id.
    /// </summary>
    public long? ParentCollection { get; set; }

    /// <summary>
    /// FK to mart_collection_relation_types.id.
    /// </summary>
    public long? Type { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual Collection? ChildCollectionNavigation { get; set; }

    public virtual Collection? ParentCollectionNavigation { get; set; }

    public virtual CollectionRelationType? TypeNavigation { get; set; }
}
