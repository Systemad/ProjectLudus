namespace Shared.Queries;

public static class CompanyQuery
{
    public static string Url = "companies";
    public static string Count = "companies/count";

    public static List<string> Fields = new()
    {
        "change_date",
        "change_date_format.format",
        "changed_company_id",
        "checksum",
        "country",
        "created_at",
        "description",
        "developed",
        "logo.image_id",
        "logo.url",
        "name",
        "parent",
        "published",
        "slug",
        "start_date",
        "start_date_format.format",
        "status",
        "updated_at",
        "url",
        "websites.type.type",
        "websites.url",
        "websites.trusted",
    };
}
