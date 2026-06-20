using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// collection_types lookup table.
/// </summary>
public partial class CollectionType
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual ICollection<CollectionMembershipType> CollectionMembershipTypes { get; set; } = new List<CollectionMembershipType>();

    public virtual ICollection<CollectionRelationType> CollectionRelationTypeAllowedChildTypeNavigations { get; set; } = new List<CollectionRelationType>();

    public virtual ICollection<CollectionRelationType> CollectionRelationTypeAllowedParentTypeNavigations { get; set; } = new List<CollectionRelationType>();

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
}
