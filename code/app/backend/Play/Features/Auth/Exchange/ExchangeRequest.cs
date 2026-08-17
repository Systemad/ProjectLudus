using System.ComponentModel.DataAnnotations;

namespace Play.Features.Auth.Exchange;

public sealed record ExchangeRequest
{
    [Required, MinLength(1)]
    public required string Code { get; init; }

    public string? State { get; init; }
}
