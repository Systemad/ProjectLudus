namespace CatalogAPI.Features.Games.Browse.GetByReleaseDateRange;

public static class Endpoint
{
    public static async Task<IResult> HandleAsync(
        [AsParameters] Request request,
        IValidator<Request> validator,
        IGameService gameService,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var games = await gameService.GetByReleaseDateRangeAsync(
            request.Start.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            request.End.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc),
            request.Limit, cancellationToken);

        return Results.Ok(new GetByReleaseDateRangeResponse(request.Start, request.End, request.Limit, games));
    }
}
