using System;
using System.Collections.Generic;

namespace Data.Models;

/// <summary>
/// Join table linking games to their DLCs.
/// </summary>
public partial class GamesDlc
{
    public long? DlcSourceId { get; set; }

    public long DlcGameId { get; set; }

    public virtual Game DlcGame { get; set; } = null!;
}
