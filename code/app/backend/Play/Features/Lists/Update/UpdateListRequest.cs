using Play.Infrastructure.Persistence;

namespace Play.Features.Lists.Update;

public sealed record UpdateListRequest(string Name, string? Description, ListVisibility Visibility);
