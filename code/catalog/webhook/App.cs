#!/usr/bin/env dotnet
// #:property TargetFramework=net10.0
//#:property LangVersion=preview
#:property PublishAot=false
#:property PublishSingleFile=false
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#:project ../Data/Data.csproj
#:sdk Microsoft.NET.Sdk.Web
#:include WebhookFilters.cs
#:include WebhookManager.cs

using System.Text.Json;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDatabaseConfigurations("catalogdb");
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
    async (
        HttpRequest request,
        AppDbContext db,
        IOptions<IGDBOptions> options,
        ILogger<Program> logger
    ) =>
    {
        //var payload = await JsonSerializer.DeserializeAsync<object>(request.Body);
        var raw = await new StreamReader(request.Body).ReadToEndAsync();
        var entity = JsonSerializer.Deserialize<IgdbBase>(raw);

        if (entity is null)
            return Results.BadRequest();

        var endpoint = request.Headers["X-Endpoint"].ToString();
        var eventType = request.Headers["X-Operation"].ToString();

        db.WebhookEvents.Add(
            new WebhookEvent
            {
                Id = Guid.NewGuid(),
                EntityId = entity.Id,
                ReceivedAt = DateTimeOffset.UtcNow,
                Endpoint = endpoint,
                EventType = eventType,
                Payload = raw,
            }
        );
        await db.SaveChangesAsync();

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

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<WebhookEvent> WebhookEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebhookEvent>(entity =>
        {
            entity.ToTable("webhook_events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Payload).HasColumnType("jsonb");
        });
    }
}
