using System.ComponentModel.DataAnnotations;

namespace Play.Features.Lists.Common.Pagination;

public sealed class PageRequest
{
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 50)]
    public int PageSize { get; init; } = 20;
}
