namespace BuildingBlocks.Core.Pagination;

public record PaginatedResponse<T>(
    IEnumerable<T> Items,
    long TotalItemCount,
    long PageCount,
    long PageSize,
    long PageNumber,
    bool IsLastPage
);
