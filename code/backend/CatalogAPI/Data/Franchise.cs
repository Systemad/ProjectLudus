using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class Franchise
{
    public long Id { get; set; }

    public long? CreatedAt { get; set; }

    public long? UpdatedAt { get; set; }

    public string Games { get; set; } = null!;

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Url { get; set; }

    public string? Checksum { get; set; }

    public virtual ICollection<Game> GamesNavigation { get; set; } = new List<Game>();

    public virtual ICollection<Game> Games1 { get; set; } = new List<Game>();
}
