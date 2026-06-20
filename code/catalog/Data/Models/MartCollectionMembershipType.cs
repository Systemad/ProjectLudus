using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Lookup table for collection membership types.
/// </summary>
public partial class MartCollectionMembershipType
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
    /// FK to mart_collection_types.id.
    /// </summary>
    public long? AllowedCollectionType { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual ICollection<MartCollectionMembership> MartCollectionMemberships { get; set; } = new List<MartCollectionMembership>();
}
