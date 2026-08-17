using System.ComponentModel.DataAnnotations;

namespace Play.Features.Lists;

public sealed record MembershipRequest
{
    [Required, MinLength(1), MaxLength(50)]
    public required IReadOnlyList<string> GameIds { get; init; }
}
