using System.Text.Json.Serialization;

namespace Shared.Features.Games;

public class InvolvedCompanyFlat
{
    public long Id { get; set; }
    public long CompanyId { get; set; }
    public bool Developer { get; set; }
    public bool Porting { get; set; }
    public bool Publisher { get; set; }
    public bool Supporting { get; set; }
}

public class InvolvedCompanyRaw
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("company")]
    public InvolvedCompanyIdRaw Company { get; set; }

    [JsonPropertyName("developer")]
    public bool Developer { get; set; }

    [JsonPropertyName("porting")]
    public bool Porting { get; set; }

    [JsonPropertyName("publisher")]
    public bool Publisher { get; set; }

    [JsonPropertyName("supporting")]
    public bool Supporting { get; set; }
}

public class InvolvedCompanyIdRaw
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public class InvolvedCompany : IIdentifiable
{
    public long Id { get; set; }
    public Company Company { get; set; }
    public bool Developer { get; set; }
    public bool Porting { get; set; }
    public bool Publisher { get; set; }
    public bool Supporting { get; set; }
}