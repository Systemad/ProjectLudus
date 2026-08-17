using System.ComponentModel.DataAnnotations;
using Play.Infrastructure.Persistence;

namespace Play.Features.Lists.Update;

public sealed record UpdateListRequest
{
    [Required, MinLength(1), MaxLength(100)]
    public required string Name { get; init; }

    [MaxLength(500)]
    public string? Description { get; init; }

    public ListVisibility Visibility { get; init; }
}
