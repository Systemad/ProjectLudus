using Play.Infrastructure.Persistence;

namespace Play.Features.Lists.Common.Dtos;

public sealed record ListSummaryResponse(
    Guid Id,
    string Name,
    string? Description,
    ListVisibility Visibility,
    bool IsDefault,
    int ItemCount,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
