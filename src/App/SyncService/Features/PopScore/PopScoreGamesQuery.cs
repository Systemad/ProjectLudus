namespace SyncService.Features.PopScore;

public class PopScoreGamesQuery
{
    public static string Url = "popularity_primitives";

    public static List<string> Fields = new()
    {
        "game_id",
        "value",
        "popularity_type",
    };
}
