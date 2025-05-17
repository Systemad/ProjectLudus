namespace Ludus.Shared.Features.Games;

public class GameDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ArtworkImageId { get; set; }
    public string CoverImageId { get; set; }
    public long FirstReleaseDate { get; set; }
    public string Publisher { get; set; }
    public List<string> Platforms { get; set; }
    public List<DateTime> ReleaseDates { get; set; }
    public string GameType { get; set; }
}

public record GameDetail(Game Game, IEnumerable<GameDTO> SimilarGames);
