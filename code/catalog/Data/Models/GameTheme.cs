using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GameTheme
{
    public long GameId { get; set; }

    public long ThemeId { get; set; }

    public virtual Game Game { get; set; } = null!;
}
