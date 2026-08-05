using System.ComponentModel.DataAnnotations;

namespace Catalog.Features.Games.Common.Pagination;

public class PageRequest
{
    [Range(1, int.MaxValue)]
    public int? Page { get; init; }

    [Range(1, 50)]
    public int? PageSize { get; init; }

    internal int PageNumber => Page ?? 1;

    internal int Size => PageSize ?? 20;
}
