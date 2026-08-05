namespace Play.Features.Lists.Common.Dtos;

public sealed record PagedListGamesResponse(
    IReadOnlyList<ListGameResponse> Games,
    int? NextPage,
    bool HasMore
)
{
    public static PagedListGamesResponse Create(
        List<ListGameResponse> results,
        int page,
        int pageSize
    )
    {
        var hasMore = results.Count > pageSize;
        return new PagedListGamesResponse(
            results.Take(pageSize).ToList(),
            hasMore ? page + 1 : null,
            hasMore
        );
    }
}
