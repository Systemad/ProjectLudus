using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class AlternativeName
{
    public long Id { get; set; }

    public string? Comment { get; set; }

    public long? Game { get; set; }

    public string Name { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public virtual Game? GameNavigation { get; set; }
}
