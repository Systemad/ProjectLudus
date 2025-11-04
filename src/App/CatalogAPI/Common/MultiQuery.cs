using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CatalogAPI.Common;

public class MultiQueryResult<T>
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty("count")]
    public int? Count { get; set; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty("result")]
    public List<T>? Data { get; set; }
}
