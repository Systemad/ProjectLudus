using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Badge;

public partial class DaisyBadge : DaisyComponentBase
{
    private static string Type = "d-badge";

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
            Sizes.XS => "d-badge-xs",
            Sizes.SM => "d-badge-sm",
            Sizes.MD => "d-badge-md",
            Sizes.LG => "d-badge-lg",
            Sizes.XL => "d-badge-xl",
            _ => "d-badge-md",
        };

    [Parameter]
    public Colors Color { get; set; } = Colors.Neutral;

    protected static string GetColor(Colors color) =>
        color switch
        {
            Colors.Neutral => "d-badge-neutral",
            Colors.Primary => "d-badge-primary",
            Colors.Secondary => "d-badge-secondary",
            Colors.Accent => "d-badge-accent",
            Colors.Info => "d-badge-info",
            Colors.Success => "d-badge-success",
            Colors.Warning => "d-badge-warning",
            Colors.Error => "d-badge-error",
            _ => "d-badge-neutral",
        };

    [Parameter]
    public Variants Variant { get; set; } = Variants.Default;

    protected static string GetVariant(Variants style) =>
        style switch
        {
            Variants.Default => "",
            Variants.Outline => "d-badge-outline",
            Variants.Dash => "d-badge-dash",
            Variants.Soft => "d-badge-soft",
            Variants.Ghost => "d-badge-ghost",
            _ => "",
        };

    [Parameter]
    public Modifiers Modifier { get; set; } = Modifiers.None;

    protected static string GetModifier(Modifiers modifier) =>
        modifier switch
        {
            Modifiers.None => "expr",
            Modifiers.Wide => "d-badge-wide",
            Modifiers.Block => "d-badge-block",
            Modifiers.Square => "d-badge-square",
            Modifiers.Circle => "d-badge-circle",
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
