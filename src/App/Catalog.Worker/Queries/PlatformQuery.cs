namespace CatalogAPI.Features.Platforms;

public static class PlatformQuery
{
    public static string Endpoint = "platforms";

    public static List<string> Fields =
    [
        "abbreviation",
        "alternative_name",
        "checksum",
        "created_at",
        "generation",
        "name",
        "platform_family",
        "platform_logo.*",
        "platform_type.*",
        "slug",
        "summary",
        "updated_at",
        "url",
        "versions",
        "websites",
    ];
}
