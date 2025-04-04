using ApiSdk;
using Ludus.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MudBlazor;
using MudBlazor.Services;
using MudExtensions.Services;
using TailwindMerge.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddTailwindMerge();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddMudExtensions();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
});
builder.Services.AddAuthorizationCore();

// Register your ApiClient
builder.Services.AddScoped(sp =>
{
    var authProvider = new AnonymousAuthenticationProvider();
    var httpClient = KiotaClientFactory.Create(new HttpClientHandler { AllowAutoRedirect = false });

    var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient)
    {
        BaseUrl = builder.HostEnvironment.BaseAddress,
    };

    return new ApiClient(adapter);
});

builder.Services.AddScoped<AuthenticationStateProvider, SteamAuthProvider>();
builder.Services.AddScoped<AuthenticatedUserService>();

var host = builder.Build();

var userService = host.Services.GetRequiredService<AuthenticatedUserService>();
await userService.InitializeAsync();

await host.RunAsync();
