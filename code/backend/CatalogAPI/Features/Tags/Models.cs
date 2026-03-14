namespace CatalogAPI.Features.Tags;

public class Tags
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public record FullTag(string Id, string Name, string GroupName, string Slug);
