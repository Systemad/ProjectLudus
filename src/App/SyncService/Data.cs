using Shared.Features.Games;

namespace SyncService;

public class InsertData
{
    public List<GameMode> GameModes = new List<GameMode>();
    public List<Genre> Genres = new List<Genre>();
    public List<Platform> Platforms = new List<Platform>();
    public List<PlayerPerspective> PlayerPerspectives = new List<PlayerPerspective>();
    public List<GameEngine> GameEngines = new List<GameEngine>();
    public List<Theme> Themes = new List<Theme>();
    public List<Keyword> Keywords = new List<Keyword>();
    public List<Franchise> Franchises = new List<Franchise>();
}
