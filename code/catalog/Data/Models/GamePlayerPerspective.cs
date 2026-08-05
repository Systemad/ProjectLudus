using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GamePlayerPerspective
{
    public long GameId { get; set; }

    public long PlayerPerspectiveId { get; set; }

    public virtual Game Game { get; set; } = null!;
}
