using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Status;

public partial class DaisyStatus : DaisyComponentBase
{
    private static string Type = "d-status";

    public enum Colors
    {
        Neutral,
        Primary,
        Secondary,
        Accent,
        Info,
        Success,
        Warning,
        Error,
    }

    public enum Sizes
    {
        XS,
        SM,
        MD,
        LG,
        XL,
    }

    public enum Animations
    {
        None,
        Ping,
        Bounce,
    }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.MD;

    protected static string GetSize(Sizes size) =>
        size switch
        {
            Sizes.XS => "d-status-xs",
            Sizes.SM => "d-status-sm",
            Sizes.MD => "d-status-md",
            Sizes.LG => "d-status-lg",
            Sizes.XL => "d-status-xl",
            _ => "d-status-md",
        };

    [Parameter]
    public Colors Color { get; set; } = Colors.Neutral;

    protected static string GetColor(Colors color) =>
        color switch
        {
            Colors.Neutral => "d-status-neutral",
            Colors.Primary => "d-status-primary",
            Colors.Secondary => "d-status-secondary",
            Colors.Accent => "d-status-accent",
            Colors.Info => "d-status-info",
            Colors.Success => "d-status-success",
            Colors.Warning => "d-status-warning",
            Colors.Error => "d-status-error",
            _ => "d-status-neutral",
        };

    [Parameter]
    public Animations Animation { get; set; } = Animations.None;

    protected static string GetAnimation(Animations animation) =>
        animation switch
        {
            Animations.None => "",
            Animations.Bounce => "animate-bounce",
            _ => "",
        };

    private protected string? Classes =>
        TwMerge.Merge(Type, GetColor(Color), GetSize(Size), GetAnimation(Animation));
}
