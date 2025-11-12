using IGDB;
using IGDB.Models;
using IGDB.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgmq;
using BuildingBlocks;
using BuildingBlocks.Twitch;

namespace Catalog.Ingester.Webhooks.Endpoints;

public static class WebhookControllerExtension
{
    public static JsonSerializerSettings DefaultJsonSerializerSettings =
        new JsonSerializerSettings()
        {
            Converters = new List<JsonConverter>()
            {
                new IdentityConverter(),
                new UnixTimestampConverter(),
            },
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            },
        };
    
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
        app.MapPost(
            $"webhooks/{resourceName}/{actionType}",
            async (
                HttpContext context,
                [FromServices] NpgmqClient npgmqClient,
                [FromServices] IConfiguration config
            ) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                
                IdentityOrValue<TResource>? deserializedPayload;
                try
                {
                    deserializedPayload = JsonConvert.DeserializeObject<IdentityOrValue<TResource>>(
                        body,
                        DefaultJsonSerializerSettings
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
                    PayloadJson = JsonConvert.SerializeObject(deserializedPayload)
                };

                await npgmqClient.SendAsync(queueName, queueMessage);
                return Results.Accepted();
            }
        );
    }
}
