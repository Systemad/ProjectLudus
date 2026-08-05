using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their standalone expansions.
/// </summary>
public partial class GameStandaloneExpansion
{
    public long? StandaloneExpansionSourceId { get; set; }

    public long StandaloneExpansionId { get; set; }

    public virtual Game StandaloneExpansion { get; set; } = null!;
}
