namespace Ludus.Client.Features.Games;

public class GameMode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }
}

public static class GameModeFilters
{
    public static readonly List<GameMode> All = new()
    {
        new GameMode
        {
            Id = 1,
            Name = "Single player",
            Slug = "single-player",
            Url = "https://www.igdb.com/game_modes/single-player",
        },
        new GameMode
        {
            Id = 2,
            Name = "Multiplayer",
            Slug = "multiplayer",
            Url = "https://www.igdb.com/game_modes/multiplayer",
        },
        new GameMode
        {
            Id = 3,
            Name = "Co-operative",
            Slug = "co-operative",
            Url = "https://www.igdb.com/game_modes/co-operative",
        },
        new GameMode
        {
            Id = 4,
            Name = "Split screen",
            Slug = "split-screen",
            Url = "https://www.igdb.com/game_modes/split-screen",
        },
        new GameMode
        {
            Id = 5,
            Name = "Massively Multiplayer Online (MMO)",
            Slug = "massively-multiplayer-online-mmo",
            Url = "https://www.igdb.com/game_modes/massively-multiplayer-online-mmo",
        },
        new GameMode
        {
            Id = 6,
            Name = "Battle Royale",
            Slug = "battle-royale",
            Url = "https://www.igdb.com/game_modes/battle-royale",
        },
    };
}
