using Microsoft.AspNetCore.Mvc;
using Npgmq;

namespace Catalog.Ingester.Webhooks;

//



public static class WebhookEndpoints
{
    public static void MapWebhookEndpoints(WebApplication app, [FromServices] NpgmqClient npgmqClient, [FromServices] IConfiguration config)
    {
        app.MapPost("/webhooks/{resource}/{actionType}", async (string resource, string actionType, HttpRequest request) =>
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

            // TODO: Validate secret header and deserialize JSON payload

            switch (resource.ToLower())
            {
                case "games":
                    // Handle game webhook for actionType (create, update, delete)
                    break;
                case "companies":
                    // Handle company webhook for actionType
                    break;
                default:
                    return Results.BadRequest("Unknown resource");
            }

            return Results.Ok();
        });
    }
}