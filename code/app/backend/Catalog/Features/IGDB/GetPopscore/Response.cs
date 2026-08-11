using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.IGDB.GetPopscore;

public sealed class Request : PageRequest
{
    public string? PopularityTypeId { get; init; }

    public DateTime? From { get; init; }

    public DateTime? To { get; init; }

}
