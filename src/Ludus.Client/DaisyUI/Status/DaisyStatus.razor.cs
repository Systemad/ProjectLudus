using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Status;

public partial class DaisyStatus : DaisyComponentBase
{
    private static string Type = "tw:d-status";

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
            Sizes.XS => "tw:d-status-xs",
            Sizes.SM => "tw:d-status-sm",
            Sizes.MD => "tw:d-status-md",
            Sizes.LG => "tw:d-status-lg",
            Sizes.XL => "tw:d-status-xl",
            _ => "tw:d-status-md",
        };

    [Parameter]
    public Colors Color { get; set; } = Colors.Neutral;

    protected static string GetColor(Colors color) =>
        color switch
        {
            Colors.Neutral => "tw:d-status-neutral",
            Colors.Primary => "tw:d-status-primary",
            Colors.Secondary => "tw:d-status-secondary",
            Colors.Accent => "tw:d-status-accent",
            Colors.Info => "tw:d-status-info",
            Colors.Success => "tw:d-status-success",
            Colors.Warning => "tw:d-status-warning",
            Colors.Error => "tw:d-status-error",
            _ => "tw:d-status-neutral",
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
