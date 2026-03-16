using System;
using System.Collections.Generic;
using NodaTime;

namespace CatalogAPI.Data;

/// <summary>
/// Latest popularity snapshot per game, popularity type, and external source.
/// </summary>
public partial class GamePopularityLatest
{
    /// <summary>
    /// Source popularity primitive id selected as latest for the key.
    /// </summary>
    public string? Id { get; set; }

    public long GameId { get; set; }

    public long PopularityType { get; set; }

    public long ExternalPopularitySource { get; set; }

    public double? Value { get; set; }

    public long? CalculatedAt { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public Instant? UpdatedAtTz { get; set; }

    public LocalDate? SnapshotDate { get; set; }

    public virtual ExternalGameSource ExternalPopularitySourceNavigation { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual PopularityType PopularityTypeNavigation { get; set; } = null!;
}
