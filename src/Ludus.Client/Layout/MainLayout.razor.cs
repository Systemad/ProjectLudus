using Ludus.Client.Features.Loading;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ludus.Client.Layout;

public partial class MainLayout : LayoutComponentBase
{
    [Inject]
    public AuthenticatedUserService UserService { get; set; }

    [Inject]
    public LoadingService LoadingService { get; set; }
    private bool _drawerOpen = true;
    private bool _isDarkMode = false;
    private MudTheme? _theme = null;

    public string GetImageSrc(string contentType, byte[] content)
    {
        var base64 = Convert.ToBase64String(content);
        return $"data:{contentType};base64,{base64}";
    }

    protected override async Task OnInitializedAsync()
    {
        _theme = new()
        {
            //PaletteLight = _lightPalette,
            //PaletteDark = _darkPalette,
            LayoutProperties = new LayoutProperties(),
        };
        // disposable?
        LoadingService.PropertyChanged += (_, __) =>
        {
            //await Task.Delay(2000);
            StateHasChanged();
        };
        await base.OnInitializedAsync();
    }

    protected override void OnInitialized() { }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void DarkModeToggle()
    {
        _isDarkMode = !_isDarkMode;
    }

    /*
        private readonly PaletteLight _lightPalette = new()
        {
            Primary = "#1E6A4F", // --md-sys-color-primary (rgb(30 106 79))
            PrimaryLighten = "#A8F2CF", // --md-sys-color-primary-container (rgb(168 242 207))
            PrimaryContrastText = "#FFFFFF", // --md-sys-color-on-primary
    
            Secondary = "#4D6358", // --md-sys-color-secondary
            SecondaryLighten = "#CFE9DA", // --md-sys-color-secondary-container
            SecondaryContrastText = "#FFFFFF", // --md-sys-color-on-secondary
    
            Tertiary = "#3E6374", // --md-sys-color-tertiary
            TertiaryLighten = "#C1E8FC", // --md-sys-color-tertiary-container
            TertiaryContrastText = "#FFFFFF", // --md-sys-color-on-tertiary
    
            Error = "#BA1A1A", // --md-sys-color-error
            ErrorLighten = "#FFDAD6", // --md-sys-color-error-container
            ErrorContrastText = "#FFFFFF", // --md-sys-color-on-error
    
            Background = "#F5FBF5", // --md-sys-color-background
            Surface = "#F5FBF5", // --md-sys-color-surface
            //SurfaceLight = "#F5FBF5", // Same as background for M3
            //SurfaceDark = "#E4EAE4", // --md-sys-color-surface-container-high
            TextPrimary = "#171D1A", // --md-sys-color-on-background
            TextSecondary = "#404944", // --md-sys-color-on-surface-variant
    
            DrawerBackground = "#FFFFFF", // Optional override if you want pure white
            AppbarBackground = "#F5FBF5CC", // surface with transparency
            AppbarText = "#404944", // --md-sys-color-on-surface-variant
            LinesDefault = "#BFC9C2", // --md-sys-color-outline-variant
            Divider = "#BFC9C2", // Same as outline-variant
            GrayLight = "#EAEFEA", // --md-sys-color-surface-container
            GrayLighter = "#EFF5EF", // --md-sys-color-surface-container-low
        };
    
        private readonly PaletteDark _darkPalette = new()
        {
            Primary = "#7e6fff",
            Surface = "#1e1e2d",
            Background = "#1a1a27",
            BackgroundGray = "#151521",
            AppbarText = "#92929f",
            AppbarBackground = "rgba(26,26,39,0.8)",
            DrawerBackground = "#1a1a27",
            ActionDefault = "#74718e",
            ActionDisabled = "#9999994d",
            ActionDisabledBackground = "#605f6d4d",
            TextPrimary = "#b2b0bf",
            TextSecondary = "#92929f",
            TextDisabled = "#ffffff33",
            DrawerIcon = "#92929f",
            DrawerText = "#92929f",
            GrayLight = "#2a2833",
            GrayLighter = "#1e1e2d",
            Info = "#4a86ff",
            Success = "#3dcb6c",
            Warning = "#ffb545",
            Error = "#ff3f5f",
            LinesDefault = "#33323e",
            TableLines = "#33323e",
            Divider = "#292838",
            OverlayLight = "#1e1e2d80",
        };
    */
    public string DarkLightModeButtonIcon =>
        _isDarkMode switch
        {
            true => Icons.Material.Rounded.AutoMode,
            false => Icons.Material.Outlined.DarkMode,
        };
}
