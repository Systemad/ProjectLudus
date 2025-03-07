namespace Shared;

public partial class CollectionRelationTypes
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    // TODO: Collections model here?
    [JsonPropertyName("allowed_child_type")]
    public long AllowedChildType { get; set; }

    [JsonPropertyName("allowed_parent_type")]
    public long AllowedParentType { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }
}