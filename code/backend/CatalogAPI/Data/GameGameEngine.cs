namespace CatalogAPI.Data;

/// <summary>
/// Join table linking games to game engines in a relational format.
/// </summary>
public partial class GameGameEngine
{
    /// <summary>
    /// The game identifier.
    /// </summary>
    public long? GameId { get; set; }

    /// <summary>
    /// The game engine identifier.
    /// </summary>
    public long? GameEngineId { get; set; }
}
