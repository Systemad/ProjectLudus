namespace CatalogAPI.Features.Themes;

public static class ThemeQuery
{
    public static string Endpoint = "themes";

    public static List<string> Fields = ["slug", "name", "created_at", "updated_at", "url"];
}
