using System.ComponentModel.DataAnnotations;
using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.IGDB.GetPopscore;

public sealed class Request : PageRequest
{
    [Range(1, long.MaxValue)]
    public long? PopularityTypeId { get; init; }

    public DateTime? From { get; init; }

    public DateTime? To { get; init; }

    internal long PopularityType => PopularityTypeId ?? 9;
}
