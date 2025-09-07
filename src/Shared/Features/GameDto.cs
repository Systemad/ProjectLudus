using Shared.Features.Games;

namespace Shared.Features;

public class GameDto
{
    public long Id { get; set; }
    public List<Artwork> Artworks { get; set; } = new();
    public Cover? Cover { get; set; }
    public long FirstReleaseDate { get; set; }
    //public List<GameEngine> GameEngines { get; set; } = new();
    public List<GameMode> GameModes { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();
    public required string Name { get; set; }
    public List<Platform> Platforms { get; set; } = new();
    //public List<PlayerPerspective> PlayerPerspectives { get; set; } = new();
    //public List<Theme> Themes { get; set; } = new();
    //public List<MultiplayerMode> MultiplayerModes { get; set; } = new();
}