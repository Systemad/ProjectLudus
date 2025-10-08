namespace SyncService.Features.Platforms;

public static class PlatformQuery
{
    public static string Endpoint = "platforms";

    public static List<string> Fields =
    [
        "fields",
        "abbreviation",
        "alternative_name",
        "generation",
        "name",
        "platform_logo.url",
        "platform_logo.image_id",
        "slug",
        "summary",
        "created_at",
        "updated_at",
        "url",
        "websites.url",
        "websites.trusted"
    ];
}