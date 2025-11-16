namespace Catalog.Worker.Queries;

public static class CompanyQuery
{
    public static string Endpoint = "companies";

    public static List<string> Fields =
    [
        "abbreviation",
        "alternative_name",
        "generation",
        "name",
        "platform_logo.url",
        "platform_logo.image_id",
        "slug",
        "summary",
        "updated_at",
        "url",
        "websites.url",
        "websites.trusted",
    ];
}
