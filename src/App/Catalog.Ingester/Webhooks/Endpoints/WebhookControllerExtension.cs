using System.Text.Json;
using IGDB;
using IGDB.Models;
using Microsoft.AspNetCore.Mvc;
using Npgmq;
using Shared;
using Shared.Features;

namespace Catalog.Ingester.Webhooks.Endpoints;

public static class WebhookControllerExtension
{
    public static void MapWebhookController<TResource>(
        this WebApplication app,
        IgdbType resourceType,
        EventType eventType,
        string resourceName,
        string actionType,
        string queueName
    )
        where TResource : class, IIdentifier
    {
        var sanitizedResourceName = resourceName.Trim('/');
        
        app.MapPost(
            $"/webhooks/{sanitizedResourceName}/{{actionType}}",
            async (
                HttpRequest request,
                [FromServices] NpgmqClient npgmqClient,
                [FromServices] IConfiguration config
            ) =>
            {
                IdentityOrValue<TResource>? deserializedPayload;
                try
                {
                    deserializedPayload = JsonSerializer.Deserialize<IdentityOrValue<TResource>>(
                        request.Body,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        }
                    );
                }
                catch (JsonException e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                if (deserializedPayload == null)
                {
                    return Results.BadRequest("Unable to deserialize payload.");
                }
                
                long resourceId;
                if (deserializedPayload.Value?.Id != null)
                {
                    resourceId = deserializedPayload.Value.Id.Value;
                }
                else if (deserializedPayload.Id.HasValue)
                {
                    resourceId = deserializedPayload.Id.Value;
                }
                else
                {
                    return Results.BadRequest("Payload missing resource ID or object.");
                }

                var queueMessage = new WebhookEvent
                {
                    ResourceId = resourceId,
                    ResourceType = resourceType,
                    EventType = eventType,
                    PayloadJson = JsonSerializer.Serialize(deserializedPayload),
                };

                await npgmqClient.SendAsync(queueName, queueMessage);
                return Results.Accepted();
            }
        );
    }
}
