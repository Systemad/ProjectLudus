namespace Play.Features.Auth.Exchange;

public sealed record AuthExchangeResponse(string Code, string? State);
