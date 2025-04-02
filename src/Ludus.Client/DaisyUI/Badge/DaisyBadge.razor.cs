using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Badge;

public partial class DaisyBadge : DaisyComponentBase
{
    private static string Type = "tw:d-badge";

    public enum Modifiers
    {
        None,
        Wide,
        Block,
        Square,
        Circle,
    }

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

    public enum Variants
    {
        Default,
        Outline,
        Dash,
        Soft,
        Ghost,
    }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.MD;

    protected static string GetSize(Sizes size) =>
        size switch
        {
            Sizes.XS => "tw:d-badge-xs",
            Sizes.SM => "tw:d-badge-sm",
            Sizes.MD => "tw:d-badge-md",
            Sizes.LG => "tw:d-badge-lg",
            Sizes.XL => "tw:d-badge-xl",
            _ => "d-badge-md",
        };

    [Parameter]
    public Colors Color { get; set; } = Colors.Neutral;

    protected static string GetColor(Colors color) =>
        color switch
        {
            Colors.Neutral => "tw:d-badge-neutral",
            Colors.Primary => "tw:d-badge-primary",
            Colors.Secondary => "tw:d-badge-secondary",
            Colors.Accent => "tw:d-badge-accent",
            Colors.Info => "tw:d-badge-info",
            Colors.Success => "tw:d-badge-success",
            Colors.Warning => "tw:d-badge-warning",
            Colors.Error => "tw:d-badge-error",
            _ => "tw:d-badge-neutral",
        };

    [Parameter]
    public Variants Variant { get; set; } = Variants.Default;

    protected static string GetVariant(Variants style) =>
        style switch
        {
            Variants.Default => "",
            Variants.Outline => "tw:d-badge-outline",
            Variants.Dash => "tw:d-badge-dash",
            Variants.Soft => "tw:d-badge-soft",
            Variants.Ghost => "tw:d-badge-ghost",
            _ => "",
        };

    [Parameter]
    public Modifiers Modifier { get; set; } = Modifiers.None;

    protected static string GetModifier(Modifiers modifier) =>
        modifier switch
        {
            Modifiers.None => "",
            Modifiers.Wide => "tw:d-badge-wide",
            Modifiers.Block => "tw:d-badge-block",
            Modifiers.Square => "tw:d-badge-square",
            Modifiers.Circle => "tw:d-badge-circle",
            _ => "",
        };

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private protected string? Classes =>
        TwMerge.Merge(
            Type,
            GetVariant(Variant),
            GetColor(Color),
            GetSize(Size),
            GetModifier(Modifier)
        );
}
