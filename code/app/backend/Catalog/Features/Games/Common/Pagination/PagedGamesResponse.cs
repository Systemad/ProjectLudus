using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Common.Pagination;

public sealed record PagedGamesResponse(List<GameBrowseDto> Games, int? NextPage, bool HasMore)
{
    internal static PagedGamesResponse Create(List<GameBrowseDto> results, int page, int pageSize)
    {
        var hasMore = results.Count > pageSize;

        return new PagedGamesResponse(
            results.Take(pageSize).ToList(),
            hasMore ? page + 1 : null,
            hasMore
        );
    }
}
