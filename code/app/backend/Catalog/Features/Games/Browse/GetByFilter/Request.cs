using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Games.Browse.GetByFilter;

public sealed class Request : PageRequest
{
    public string? Genre { get; init; }

    public string? Theme { get; init; }

    public string? GameMode { get; init; }

    public DateTime? From { get; init; }

    public DateTime? To { get; init; }
}
