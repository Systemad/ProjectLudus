using Play.Infrastructure.Persistence;

namespace Play.Features.Lists.Create;

public sealed record CreateListRequest(
    string Name,
    string? Description,
    ListVisibility Visibility = ListVisibility.Private
);
