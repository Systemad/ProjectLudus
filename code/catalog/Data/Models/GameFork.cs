using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their forks.
/// </summary>
public partial class GameFork
{
    public long? ForkSourceId { get; set; }

    public long ForkId { get; set; }

    public virtual Game Fork { get; set; } = null!;
}
