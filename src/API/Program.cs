using System.Security.Claims;
using API;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:7152")  // Your Blazor client URL
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/signin";
        options.LogoutPath = "/signout";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.Events.OnSignedIn = ValidationHelper.SignIn;
        options.Events.OnValidatePrincipal = ValidationHelper.Validate;
    })
    .AddSteam();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();
// Output cache
builder.Services.AddOutputCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseCors("AllowBlazorApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("api/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("~/signin", ([FromQuery] string returnUrl = "/") =>
{
    return Results.Challenge(new AuthenticationProperties
    {
        RedirectUri = "https://localhost:7152" + returnUrl,
        IsPersistent = true,
        IssuedUtc = DateTime.Now,
        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
    }, ["Steam"]);
});

app.MapPost("~/signin", ([FromQuery] string returnUrl = "/") =>
{
    return Results.Challenge(new AuthenticationProperties
    {
        RedirectUri = "https://localhost:7152" + returnUrl,
        IsPersistent = true,
        IssuedUtc = DateTime.Now,
        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
    }, ["Steam"]);
});

app.MapGet("api/me", async Task<Results<Ok<LudusUser>, UnauthorizedHttpResult>> (ClaimsPrincipal User) =>
{
    if (User.Identity?.IsAuthenticated ?? false)
    {
        var db = new AppDbContext();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(User.Identity.Name));
        return TypedResults.Ok(user);
    }
    return TypedResults.Unauthorized();
});

app.UseOutputCache();

app.MapRazorPages();
app.MapControllers();

//app.MapReverseProxy();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}