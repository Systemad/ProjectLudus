using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Links games to collections with a membership type.
/// </summary>
public partial class MartCollectionMembership
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// FK to mart_games.id.
    /// </summary>
    public long? Game { get; set; }

    /// <summary>
    /// FK to mart_collections.id.
    /// </summary>
    public long? Collection { get; set; }

    /// <summary>
    /// FK to mart_collection_membership_types.id.
    /// </summary>
    public long? Type { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual MartCollection? CollectionNavigation { get; set; }

    public virtual MartCollectionMembershipType? TypeNavigation { get; set; }
}
