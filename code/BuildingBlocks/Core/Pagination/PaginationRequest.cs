namespace BuildingBlocks.Core.Pagination;

public abstract class PaginationRequest
{
    [QueryParam, BindFrom("pageSize")]
    public int? _pageSize { get; set; }

    [QueryParam, BindFrom("pageNumber")]
    public int? _pageNumber { get; set; }

    [JsonIgnore]
    public int PageSize => _pageSize ?? 40;

    [JsonIgnore]
    public int PageNumber => _pageNumber ?? 1;
}
