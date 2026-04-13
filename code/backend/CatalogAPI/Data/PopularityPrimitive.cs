namespace CatalogAPI.Data;

public partial class PopularityPrimitive
{
    public string Id { get; set; } = null!;

    public long? GameId { get; set; }

    public long? PopularityType { get; set; }

    public double? Value { get; set; }

    public long? CalculatedAt { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public long? ExternalPopularitySource { get; set; }

    public Instant? UpdatedAtTz { get; set; }

    public LocalDate? SnapshotDate { get; set; }

    public virtual ExternalGameSource? ExternalPopularitySourceNavigation { get; set; }

    public virtual Game? Game { get; set; }

    public virtual PopularityType? PopularityTypeNavigation { get; set; }
}
