using System.ComponentModel.DataAnnotations;
using PlayAPI.Data;

namespace PlayAPI.Features.Games.Analytics.RecordEvent;

public static class RecordGameEventEndpoints
{
    public sealed record RecordGameEventRequest(
        long GameId,
        GameEventType EventType,
        string SessionId
    );

    public sealed record RecordGameEventResponse(
        [Required] long GameId,
        [Required] GameEventType EventType,
        [Required] Guid SessionId,
        [Required] DateTime CreatedAt
    );

    public static IEndpointRouteBuilder MapRecordGameEventEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/events");

        group
            .MapPost("", HandleAsync)
            .WithName("GamesAnalytics/RecordGameEvent")
            .Produces<RecordGameEventResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        RecordGameEventRequest request,
        RecordGameEventService service,
        CancellationToken cancellationToken
    )
    {
        var validationErrors = new Dictionary<string, string[]>();

        if (request.GameId <= 0)
        {
            validationErrors[nameof(request.GameId)] = ["GameId must be greater than 0."];
        }

        if (string.IsNullOrWhiteSpace(request.SessionId))
        {
            validationErrors[nameof(request.SessionId)] = ["SessionId is required."];
        }

        if (!Guid.TryParse(request.SessionId, out var sessionId))
        {
            validationErrors[nameof(request.SessionId)] =
            [
                "SessionId must be a valid UUID."
            ];
        }

        if (validationErrors.Count > 0)
        {
            return Results.ValidationProblem(validationErrors);
        }

        var result = await service.RecordAsync(
            request.GameId,
            request.EventType,
            sessionId,
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
