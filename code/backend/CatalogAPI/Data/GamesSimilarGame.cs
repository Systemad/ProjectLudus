using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

/// <summary>
/// Join table representing the similar-games relationship between games.
/// </summary>
public partial class GamesSimilarGame
{
    /// <summary>
    /// The source game in the similarity relationship.
    /// </summary>
    public long? GameId { get; set; }

    /// <summary>
    /// The related similar game.
    /// </summary>
    public long? SimilarGameId { get; set; }
}
