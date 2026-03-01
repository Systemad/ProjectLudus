using System.Text.Json.Serialization;

namespace CatalogAPI;

public abstract class PaginationRequest
{

    public int? _pageSize { get; set; }

  
    public int? _pageNumber { get; set; }

    [JsonIgnore]
    public int PageSize => _pageSize ?? 40;

    [JsonIgnore]
    public int PageNumber => _pageNumber ?? 1;
}
