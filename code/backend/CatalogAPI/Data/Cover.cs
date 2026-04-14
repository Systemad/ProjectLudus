using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Cover
{
    public long Id { get; set; }

    public long? GameId { get; set; }

    public bool? AlphaChannel { get; set; }

    public bool? Animated { get; set; }

    public long? Height { get; set; }

    public string? ImageId { get; set; }

    public string? Url { get; set; }

    public long? Width { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<GameLocalization> GameLocalizations { get; set; } = new List<GameLocalization>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
