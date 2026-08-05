namespace Play.Features.Auth.Exchange;

public sealed record ExchangeRequest(string Code, string? State);
