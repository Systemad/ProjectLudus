using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their ports.
/// </summary>
public partial class GamePort
{
    public long? PortSourceId { get; set; }

    public long PortId { get; set; }

    public virtual Game Port { get; set; } = null!;
}
