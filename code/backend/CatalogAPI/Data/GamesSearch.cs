using System;
using System.Collections.Generic;

namespace CatalogAPI.Data;

public partial class GamesSearch
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Summary { get; set; }

    public string? Storyline { get; set; }

    public long? FirstReleaseDate { get; set; }

    public long? GameType { get; set; }

    public string? CoverUrl { get; set; }

    public List<string>? Themes { get; set; }

    public List<string>? Genres { get; set; }

    public List<string>? Modes { get; set; }

    public int? ReleaseYear { get; set; }
}
