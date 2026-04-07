# PlayAPI Vertical Slice Spec

This document defines the preferred structure for new PlayAPI features.

The goal is to keep each feature self-contained, easy to find, and easy to extend without growing a large shared `Extensions` or `Services` layer.

## Core Rules

1. Group code by domain first.
2. Group code by sub-domain second, only when it adds clarity.
3. Group code by use case or endpoint slice last.
4. Keep registration close to the feature it belongs to.
5. Avoid generic, catch-all files when a more specific name is possible.

## Preferred Folder Shape

```text
PlayAPI/
  Features/
    Games/
      Analytics/
        AnalyticsServiceExtensions.cs
        MapAnalyticEndpoints.cs
        RecordClick/
          RecordClickEndpoints.cs
          GameClickTrackingService.cs
    Companies/
      Search/
        SearchCompaniesEndpoints.cs
        SearchCompaniesService.cs
```

## Folder Intent

`Features/<Domain>/`

- The top-level product or API domain, for example `Games` or `Companies`.

`Features/<Domain>/<SubDomain>/`

- Optional grouping layer.
- Use this only when the domain has multiple related slices.
- Example: `Games/Analytics`.

`Features/<Domain>/<SubDomain>/<Slice>/`

- The concrete use case.
- This is usually the most important folder.
- Example: `RecordClick`, `GetGameById`, `SearchCompanies`.

## Naming Conventions

Use names that describe the behavior, not the technical abstraction.

Preferred examples:

- `RecordClickEndpoints.cs`
- `GameClickTrackingService.cs`
- `AnalyticsServiceExtensions.cs`
- `MapAnalyticEndpoints.cs`

Avoid overly generic names such as:

- `DependencyInjection.cs`
- `Endpoints.cs`
- `Services.cs`

Those names are acceptable only when they are still local to a very small, well-scoped folder, but the preferred direction in PlayAPI is more explicit naming.

## Registration Pattern

Registration should happen near the feature and be composed in `Program.cs`.

Current preferred pattern:

```csharp
builder.Services.AddGamesAnalyticsServices();
builder.Services.AddTypesenseServices(builder.Configuration);

app.MapDefaultEndpoints();
app.MapGamesAnalyticsEndpoints();
app.MapTypesenseEndpointsGroup();
```

Guideline:

- Service registration belongs in a `*ServiceExtensions.cs` file.
- Endpoint mapping belongs in a dedicated `Map*Endpoints.cs` file.
- `Program.cs` should stay composition-focused and not know implementation details.
- Methods should include short comments when they capture a design choice or boundary decision.

## Slice Design Rules

For a single slice, keep the smallest useful set of files.

Typical minimal slice:

```text
RecordClick/
  RecordClickEndpoints.cs
  GameClickTrackingService.cs
```

Add more files only when the slice actually needs them:

- Request models
- Response models
- Validators
- Handlers
- Mapping helpers

Do not split a simple slice into many tiny files unless complexity justifies it.

## Endpoint Rules

Endpoint files should:

1. Own route mapping for the slice.
2. Use minimal API handlers.
3. Delegate real work to a service or handler.
4. Keep HTTP concerns near the endpoint.

Example from the current pattern:

```csharp
public static class RecordClickEndpoints
{
    public static IEndpointRouteBuilder MapRecordClickEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games");

        group.MapPost("/{gameId:long}/clicks", RecordGameClickAsync);

        return routeBuilder;
    }
}
```

Good endpoint responsibilities:

- Route definitions
- Status code metadata
- Request validation at the HTTP boundary
- Calling the slice service
- Returning typed results

Avoid putting data access and business logic directly in endpoint mapping files.

## Service Rules

Service files should contain the slice logic.

Good service responsibilities:

- Database access
- Business rules
- State changes
- Returning a simple result back to the endpoint

Avoid letting service files become a dumping ground shared across unrelated features.

If logic is only used by one slice, keep it in that slice.

## When To Add A Sub-Domain Folder

Add a layer like `Analytics` only if it improves clarity.

Use it when:

- You expect multiple analytics-related slices.
- The domain would otherwise become crowded.
- The grouped slices share a clear purpose.

Do not add it when there is only one small slice and no likely neighbors.

Example:

- Good: `Games/Analytics/RecordClick`
- Too deep too early: `Games/Analytics/Clicks/RecordEvent`

Prefer the shallowest structure that still reads clearly.

## Recommended Workflow For New Features

When adding a new feature:

1. Pick the domain folder.
2. Decide whether a sub-domain folder adds clarity.
3. Create a slice folder named after the use case.
4. Add the endpoint file for the HTTP contract.
5. Add the service or handler file for the business logic.
6. Register services in the local `*ServiceExtensions.cs` file.
7. Register endpoint mapping in the local `Map*Endpoints.cs` file.
8. Wire the feature into `Program.cs`.

## New Feature Checklist

- Is the folder named after the domain?
- Is the slice named after the actual behavior?
- Is the endpoint mapping in a dedicated endpoint file?
- Is business logic outside the endpoint file?
- Is registration near the feature?
- Is `Program.cs` only composing features?
- Is the structure still shallow enough to scan quickly?

## Example Templates

### Service extension template

```csharp
namespace PlayAPI.Features.Games.Analytics;

public static class AnalyticsServiceExtensions
{
    public static IServiceCollection AddGamesAnalyticsServices(this IServiceCollection services)
    {
        services.AddScoped<MySliceService>();
        return services;
    }
}
```

### Endpoint mapping extension template

```csharp
namespace PlayAPI.Features.Games.Analytics;

public static class MapAnalyticEndpoints
{
    public static IEndpointRouteBuilder MapGamesAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMySliceEndpoints();
        return app;
    }
}
```

### Slice endpoint template

```csharp
namespace PlayAPI.Features.Games.Analytics.MySlice;

public static class MySliceEndpoints
{
    public static IEndpointRouteBuilder MapMySliceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/games");

        group.MapPost("/something", HandleAsync);

        return app;
    }
}
```

## Current Preferred Standard

For PlayAPI, prefer this style for new work:

- Explicit file names over generic file names.
- Domain and sub-domain folders only where they help.
- Use-case folders for concrete behavior.
- `Program.cs` as a thin composition root.
- Feature-local registration instead of central shared registries.

## Notes

Some older or transitional features may still use more generic files such as `DependencyInjection.cs`.

That is acceptable during migration, but new work should follow the explicit pattern in this document unless there is a strong reason not to.