using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Lookup table for collection relation types.
/// </summary>
public partial class MartCollectionRelationType
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Type name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Type description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// FK to mart_collection_types.id — allowed type for child.
    /// </summary>
    public long? AllowedChildType { get; set; }

    /// <summary>
    /// FK to mart_collection_types.id — allowed type for parent.
    /// </summary>
    public long? AllowedParentType { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual ICollection<MartCollectionRelation> MartCollectionRelations { get; set; } = new List<MartCollectionRelation>();
}
