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

    public string? GameType { get; set; }

    public string? GameStatus { get; set; }

    public string? CoverUrl { get; set; }

    public List<string>? Themes { get; set; }

    public List<string>? Genres { get; set; }

    public List<string>? GameModes { get; set; }

    public List<string>? Platforms { get; set; }

    public List<string>? GameEngines { get; set; }

    public List<string>? PlayerPerspectives { get; set; }

    public List<string>? Publishers { get; set; }

    public List<string>? Developers { get; set; }

    public List<string>? MultiplayerModes { get; set; }

    public int? ReleaseYear { get; set; }
}
