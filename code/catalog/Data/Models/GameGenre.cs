using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class GameGenre
{
    public long GameId { get; set; }

    public long GenreId { get; set; }

    public virtual Game Game { get; set; } = null!;
}
