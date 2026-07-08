#!/usr/bin/env dotnet
// #:property TargetFramework=net10.0
//#:property LangVersion=preview
#:property PublishAot=false
#:property PublishSingleFile=false
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#:sdk Microsoft.NET.Sdk.Web
#:package Npgsql.DependencyInjection@10.0.3

#:include WebhookFilters.cs
#:include WebhookManager.cs
#:include WebhookModels.cs

using System.Text.Json;
using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    builder.Configuration.GetConnectionString("catalogdb")
        ?? throw new InvalidOperationException("Connection string 'catalogdb' not found.")
);

builder.Services.AddOptions<IGDBOptions>().Bind(builder.Configuration.GetSection("IGDB"));
builder.Services.AddHttpClient<WebhookManager>(
    (sp, client) =>
    {
        var options = sp.GetRequiredService<IOptions<IGDBOptions>>().Value;

        client.BaseAddress = new Uri("https://api.igdb.com/v4/");
        client.DefaultRequestHeaders.Add("Client-ID", options.CLIENT_ID);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ACCESS_TOKEN}");
    }
);

builder.Services.AddSingleton<WebhookManager>();

var app = builder.Build();
var group = app.MapGroup("").AddEndpointFilter<WebhookSecretFilter>();

group.MapPost(
    "/igdb-webhook",
    static async (
        HttpRequest request,
        NpgsqlConnection connection,
        IOptions<IGDBOptions> options,
        ILogger<Program> logger
    ) =>
    {
        var raw = await new StreamReader(request.Body).ReadToEndAsync();
        var entity = JsonSerializer.Deserialize<IgdbBase>(raw);

        if (entity is null)
            return Results.BadRequest();

        var endpoint = request.Headers["X-Endpoint"].ToString();
        var eventType = request.Headers["X-Operation"].ToString();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO igdb_source.webhook_events (id, entity_id, endpoint, event_type, payload, processed) VALUES (@p1), (@p2), (@p3), (@p4), (@p5), (@p6)",
            connection
        )
        {
            Parameters =
            {
                new("p1", Guid.NewGuid()) { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid },
                new("p2", entity.Id) { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bigint },
                new("p3", endpoint) { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text },
                new("p4", eventType) { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text },
                new("p5", raw) { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb },
                new("p6", false) { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean },
            },
        };

        await cmd.ExecuteNonQueryAsync();

        logger.LogInformation(
            "[{Time}] {EventType} {Endpoint} — {Id}",
            DateTime.UtcNow,
            eventType,
            endpoint,
            entity.Id
        );
        return Results.Ok();
    }
);

group.MapGet(
    "/admin/webhooks",
    async (HttpRequest req, WebhookManager igdb) =>
    {
        var subs = await igdb.GetAll();
        return Results.Json(subs);
    }
);

group.MapPost(
    "/admin/webhooks/sync",
    async (WebhookManager igdb) =>
    {
        await igdb.SyncAsync();
        return Results.Ok();
    }
);

group.MapPost(
    "/admin/webhooks/test",
    async (HttpRequest req, JsonElement body, WebhookManager igdb) =>
    {
        var webhookId = body.GetProperty("webhookId").GetInt32();
        var endpoint = body.GetProperty("endpoint").GetString()!;
        var entityId = body.GetProperty("entityId").GetInt32();
        var result = await igdb.Test(endpoint, webhookId, entityId);
        return Results.Content(result);
    }
);

group.MapDelete(
    "/admin/webhooks/{id:int}",
    async (HttpRequest req, int id, WebhookManager igdb) =>
    {
        await igdb.Unsubscribe(id);
        return Results.Ok();
    }
);

await app.Services.GetRequiredService<WebhookManager>().SyncAsync();
await app.RunAsync();
