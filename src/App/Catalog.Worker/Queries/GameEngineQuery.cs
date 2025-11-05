namespace CatalogAPI.Features.GameEngines;

public static class GameEngineQuery
{
    public static string Endpoint = "gameengines";

    public static List<string> Fields =
    [
        "name",
        "platforms",
        "logo.id",
        "logo.image_id",
        "logo.url",
        "logo.slug",
        "logo.url",
        "updated_at",
        "created_at",
    ];
}
