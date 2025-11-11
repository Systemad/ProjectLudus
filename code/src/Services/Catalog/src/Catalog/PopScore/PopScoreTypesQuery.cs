namespace Catalog.Api.Features.PopScore;

public class PopScoreTypesQuery
{
    public static string Url = "popularity_types";

    public static List<string> Fields = new()
    {
        "id",
        "popularity_source",
        "name",
        "updated_at",
        "external_popularity_source.name",
    };
}
