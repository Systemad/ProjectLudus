namespace CatalogAPI.Data;

public partial class GameStatus
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
