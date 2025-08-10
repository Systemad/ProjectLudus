using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public partial class Company
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("changed_company_id")]
    public long? ChangedCompanyId { get; set; }

    // ISO 3166-1
    // use `https://restcountries.com/v3.1/all?
    //  fields: 'name,ccn3'
    [JsonPropertyName("country")]
    public long Country { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("logo")]
    public Logo Logo { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonPropertyName("start_date")]
    public long? StartDate { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("websites")]
    public List<Website> Websites { get; set; }

    [JsonPropertyName("checksum")]
    public Guid Checksum { get; set; }

    [JsonPropertyName("status")]
    public CompanyStatus Status { get; set; } = new();

    [JsonPropertyName("start_date_format")]
    public StartDateFormat StartDateFormat { get; set; }

    [JsonPropertyName("change_date")]
    public long? ChangeDate { get; set; }

    [JsonPropertyName("parent")]
    public long? Parent { get; set; }
}

public partial class Logo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string ImageId { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public partial class CompanyStatus
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public partial class StartDateFormat
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }
}

public partial class Website
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("trusted")]
    public bool Trusted { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonPropertyName("type")]
    public TypeClass Type { get; set; }
}

public partial class TypeClass
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
