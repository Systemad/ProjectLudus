namespace Ludus.Client.Features.Games;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Url { get; set; }
}

public static class GenreFilters
{
    public static readonly List<Genre> All = new()
    {
        new Genre
        {
            Id = 2,
            Name = "Point-and-click",
            Slug = "point-and-click",
            Url = "https://www.igdb.com/genres/point-and-click",
        },
        new Genre
        {
            Id = 4,
            Name = "Fighting",
            Slug = "fighting",
            Url = "https://www.igdb.com/genres/fighting",
        },
        new Genre
        {
            Id = 5,
            Name = "Shooter",
            Slug = "shooter",
            Url = "https://www.igdb.com/genres/shooter",
        },
        new Genre
        {
            Id = 7,
            Name = "Music",
            Slug = "music",
            Url = "https://www.igdb.com/genres/music",
        },
        new Genre
        {
            Id = 8,
            Name = "Platform",
            Slug = "platform",
            Url = "https://www.igdb.com/genres/platform",
        },
        new Genre
        {
            Id = 9,
            Name = "Puzzle",
            Slug = "puzzle",
            Url = "https://www.igdb.com/genres/puzzle",
        },
        new Genre
        {
            Id = 10,
            Name = "Racing",
            Slug = "racing",
            Url = "https://www.igdb.com/genres/racing",
        },
        new Genre
        {
            Id = 11,
            Name = "Real Time Strategy (RTS)",
            Slug = "real-time-strategy-rts",
            Url = "https://www.igdb.com/genres/real-time-strategy-rts",
        },
        new Genre
        {
            Id = 12,
            Name = "Role-playing (RPG)",
            Slug = "role-playing-rpg",
            Url = "https://www.igdb.com/genres/role-playing-rpg",
        },
        new Genre
        {
            Id = 13,
            Name = "Simulator",
            Slug = "simulator",
            Url = "https://www.igdb.com/genres/simulator",
        },
        new Genre
        {
            Id = 14,
            Name = "Sport",
            Slug = "sport",
            Url = "https://www.igdb.com/genres/sport",
        },
        new Genre
        {
            Id = 15,
            Name = "Strategy",
            Slug = "strategy",
            Url = "https://www.igdb.com/genres/strategy",
        },
        new Genre
        {
            Id = 16,
            Name = "Turn-based strategy (TBS)",
            Slug = "turn-based-strategy-tbs",
            Url = "https://www.igdb.com/genres/turn-based-strategy-tbs",
        },
        new Genre
        {
            Id = 24,
            Name = "Tactical",
            Slug = "tactical",
            Url = "https://www.igdb.com/genres/tactical",
        },
        new Genre
        {
            Id = 25,
            Name = "Hack and slash/Beat 'em up",
            Slug = "hack-and-slash-beat-em-up",
            Url = "https://www.igdb.com/genres/hack-and-slash-beat-em-up",
        },
        new Genre
        {
            Id = 26,
            Name = "Quiz/Trivia",
            Slug = "quiz-trivia",
            Url = "https://www.igdb.com/genres/quiz-trivia",
        },
        new Genre
        {
            Id = 30,
            Name = "Pinball",
            Slug = "pinball",
            Url = "https://www.igdb.com/genres/pinball",
        },
        new Genre
        {
            Id = 31,
            Name = "Adventure",
            Slug = "adventure",
            Url = "https://www.igdb.com/genres/adventure",
        },
        new Genre
        {
            Id = 32,
            Name = "Indie",
            Slug = "indie",
            Url = "https://www.igdb.com/genres/indie",
        },
        new Genre
        {
            Id = 33,
            Name = "Arcade",
            Slug = "arcade",
            Url = "https://www.igdb.com/genres/arcade",
        },
        new Genre
        {
            Id = 34,
            Name = "Visual Novel",
            Slug = "visual-novel",
            Url = "https://www.igdb.com/genres/visual-novel",
        },
        new Genre
        {
            Id = 35,
            Name = "Card & Board Game",
            Slug = "card-and-board-game",
            Url = "https://www.igdb.com/genres/card-and-board-game",
        },
        new Genre
        {
            Id = 36,
            Name = "MOBA",
            Slug = "moba",
            Url = "https://www.igdb.com/genres/moba",
        },
    };
}
