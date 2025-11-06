using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Npgmq;
using Shared;
using Shared.Features;

namespace Catalog.Ingester.Webhooks.Endpoints;

//



public static class GameEndpoints
{
    public static void MapWebhookEndpoints(WebApplication app, [FromServices] NpgmqClient npgmqClient, [FromServices] IConfiguration config)
    {
        app.MapPost("/webhooks/games/{actionType}", async (string actionType, HttpRequest request) =>
        {
            // validate header secret!
            if (!request.Headers.TryGetValue("X-Secret", out var headerSecret) || string.IsNullOrEmpty(headerSecret))
            {
                return Results.BadRequest();
            }
            
            var hasValidHeader = config.GetValue<string>("WEBHOOK_SECRET") is { } key
                                 && headerSecret == $"Key: {key}";
            if (hasValidHeader is false)
            {
                return Results.Unauthorized();
            }
            
            // Read request body
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
                    var serialized = JsonSerializer.Deserialize<WebhookPayload>(body);

            var eventMessage = new WebhookEvent
            {
                ResourceId = 1,
                CreatedAt = 0,
                ResourceType = IgdbType.GAME,
                EventType = EventType.Created,
                PayloadJson = null,
                Timestamp = default
            };


            return Results.Ok();
        });
    }
}