using Marten;
using Shared.Features.Games;

namespace WebAPI.Features.Common.Games;

public class HydrationCache
{
    public IDocumentStore GameStore { get; set; }

    public List<Genre> Genres { get; private set; }
    public List<Platform> Platforms { get; private set; }
    public List<GameEngine> GameEngines { get; private set; }
    public List<Keyword> Keywords { get; private set; }
    public List<GameMode> GameModes { get; private set; }
    public List<PlayerPerspective> PlayerPerspectives { get; private set; }
    public List<Theme> Themes { get; private set; }

    public HydrationCache(IDocumentStore gameStore)
    {
        GameStore = gameStore;
    }

    public async Task LoadCacheAsync()
    {
        await using var session = GameStore.QuerySession();

        Genres = (await session.Query<Genre>().ToListAsync()).ToList();
        Platforms = (await session.Query<Platform>().ToListAsync()).ToList();
        GameEngines = (await session.Query<GameEngine>().ToListAsync()).ToList();
        Keywords = (await session.Query<Keyword>().ToListAsync()).ToList();
        GameModes = (await session.Query<GameMode>().ToListAsync()).ToList();
        Themes = (await session.Query<Theme>().ToListAsync()).ToList();
        PlayerPerspectives = (await session.Query<PlayerPerspective>().ToListAsync()).ToList();
    }

    public async Task<Company?> GetCompanyAsync(long id)
    {
        await using var session = GameStore.QuerySession();
        var company = await session.LoadAsync<Company>(id);
        return company;
    }
    
    public Genre? GetGenre(long id) => Genres.FirstOrDefault(g => g.Id == id);
    public Platform? GetPlatform(long id) => Platforms.FirstOrDefault(p => p.Id == id);
    public GameEngine? GetGameEngine(long id) => GameEngines.FirstOrDefault(p => p.Id == id);
    public Keyword? GetKeyword(long id) => Keywords.FirstOrDefault(g => g.Id == id);
    public GameMode? GetGameMode(long id) => GameModes.FirstOrDefault(p => p.Id == id);
    public PlayerPerspective? GetPlayerPerspective(long id) => PlayerPerspectives.FirstOrDefault(ge => ge.Id == id);
    public Theme? GetPlayerTheme(long id) => Themes.FirstOrDefault(ge => ge.Id == id);

    public List<Genre> GetGenres(IEnumerable<long> ids) => Genres.Where(t => ids.Contains(t.Id)).ToList();
    public List<Platform> GetPlatforms(IEnumerable<long> ids) => Platforms.Where(t => ids.Contains(t.Id)).ToList();

    public List<GameEngine> GetGameEngines(IEnumerable<long> ids) =>
        GameEngines.Where(t => ids.Contains(t.Id)).ToList();

    public List<Keyword> GetKeywords(IEnumerable<long> ids) => Keywords.Where(t => ids.Contains(t.Id)).ToList();
    public List<GameMode> GetGameModes(IEnumerable<long> ids) => GameModes.Where(t => ids.Contains(t.Id)).ToList();

    public List<PlayerPerspective> GetPlayerPerspectives(IEnumerable<long> ids) =>
        PlayerPerspectives.Where(t => ids.Contains(t.Id)).ToList();

    public List<Theme> GetPlayerThemes(IEnumerable<long> ids) => Themes.Where(t => ids.Contains(t.Id)).ToList();
}