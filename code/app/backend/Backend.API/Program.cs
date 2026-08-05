using Backend.API.Extensions;
using Catalog.Extensions;
using Play.Extensions;
using Scalar.AspNetCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddCatalogModule();
builder.AddPlayModule();
builder.AddUserLibraryFeature();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseForwardedHeaders();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.MapCatalogModule();
app.MapPlayModule();
app.MapUserLibraryFeature();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(
        "/docs",
        options =>
        {
            options.WithTitle("Backend API").ForceDarkMode();
            options.DisableAgent();
            options.DisableTelemetry();
        }
    );
}

app.Run();

namespace Backend.API
{
    public partial class Program;
}
