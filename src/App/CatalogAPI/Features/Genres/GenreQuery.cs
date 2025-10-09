namespace CatalogAPI.Features.Genres;

public static class GenreQuery
{
    public static string Endpoint = "genres";

    public static List<string> Fields = ["slug", "name", "created_at", "updated_at", "url"];
}
