namespace CatalogAPI.Data;

public partial class GameReleaseFormat
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Format { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<ExternalGame> ExternalGames { get; set; } = new List<ExternalGame>();
}
