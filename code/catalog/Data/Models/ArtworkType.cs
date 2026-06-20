using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class ArtworkType
{
    public long Id { get; set; }

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public string Slug { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Checksum { get; set; } = null!;
}
