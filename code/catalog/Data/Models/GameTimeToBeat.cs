using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Time-to-beats metrics for a game — how long to finish (hastily, normally, completely).
/// </summary>
public partial class GameTimeToBeat
{
    /// <summary>
    /// Primary key.
    /// </summary>
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    /// <summary>
    /// FK to mart_games.id — the game this time-to-beat record belongs to.
    /// </summary>
    public long? GameId { get; set; }

    /// <summary>
    /// Time to beat (rushing) in minutes.
    /// </summary>
    public long? Hastily { get; set; }

    /// <summary>
    /// Time to beat (average) in minutes.
    /// </summary>
    public long? Normally { get; set; }

    /// <summary>
    /// Time to beat (100% completion) in minutes.
    /// </summary>
    public long? Completely { get; set; }

    /// <summary>
    /// Number of submissions.
    /// </summary>
    public long? Count { get; set; }

    public string Checksum { get; set; } = null!;

    public virtual Game? Game { get; set; }
}
