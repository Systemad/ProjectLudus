using System.ComponentModel.DataAnnotations;
using PlayAPI.Data;
using PlayAPI.Features.Cookies;

namespace PlayAPI.Features.Games.Analytics.RecordEvent;

public static class RecordGameEventEndpoints
{
    public sealed record RecordGameEventRequest(long GameId, GameEventType EventType);

    public sealed record RecordGameEventResponse(
        [Required] long GameId,
        [Required] GameEventType EventType,
        [Required] Guid SessionId,
        [Required] DateTime CreatedAt
    );

    public static IEndpointRouteBuilder MapRecordGameEventEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/play/events");

        group
            .MapPost("", HandleAsync)
            .AddEndpointFilter<ConsentFilter>()
            .WithName("GamesAnalytics/RecordGameEvent")
            .Produces<RecordGameEventResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        RecordGameEventRequest request,
        RecordGameEventService service,
        ICookieService cookieService,
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        var validationErrors = new Dictionary<string, string[]>();
        if (request.GameId <= 0)
        {
            validationErrors[nameof(request.GameId)] = ["GameId must be greater than 0."];
        }

        if (validationErrors.Count > 0)
        {
            return Results.ValidationProblem(validationErrors);
        }

        var sessionId = cookieService.GetSessionId(httpContext);
        if (!sessionId.HasValue)
        {
            return Results.NoContent();
        }

        var result = await service.RecordAsync(
            request.GameId,
            request.EventType,
            sessionId.Value,
            cancellationToken
        );

        return Results.Ok(
            new RecordGameEventResponse(
                result.GameId,
                result.EventType,
                result.SessionId,
                result.CreatedAt.ToDateTimeUtc()
            )
        );
    }
}
