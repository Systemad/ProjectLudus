using Play.Infrastructure.Persistence;

namespace Play.Features.Lists.Common.Dtos;

public sealed record ListHistoryEntryResponse(
    long Id,
    Guid ListId,
    string GameId,
    ListAction Action,
    DateTimeOffset CreatedAt
);
