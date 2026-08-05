namespace Play.Features.Lists.Common.Dtos;

public sealed record GameListMembershipResponse(IReadOnlyList<Guid> ListIds, bool IsWishlisted);
