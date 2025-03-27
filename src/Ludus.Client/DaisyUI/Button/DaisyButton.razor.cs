using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Button;

public partial class DaisyButton : DaisyComponentBase
{
    private static string Type = "d-btn";

    public enum Modifiers
    {
        None,
        Wide,
        Block,
        Square,
        Circle,
    }

    public enum Behaviors
    {
        Default,
        Active,
        Disabled,
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
        Link,
    }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.MD;

    protected static string GetSize(Sizes size) =>
        size switch
        {
            Sizes.XS => "d-btn-xs",
            Sizes.SM => "d-btn-sm",
            Sizes.MD => "d-btn-md",
            Sizes.LG => "d-btn-lg",
            Sizes.XL => "d-btn-xl",
            _ => "d-btn-md",
        };

    [Parameter]
    public Colors Color { get; set; } = Colors.Neutral;

    protected static string GetColor(Colors color) =>
        color switch
        {
            Colors.Neutral => "d-btn-neutral",
            Colors.Primary => "d-btn-primary",
            Colors.Secondary => "d-btn-secondary",
            Colors.Accent => "d-btn-accent",
            Colors.Info => "d-btn-info",
            Colors.Success => "d-btn-success",
            Colors.Warning => "d-btn-warning",
            Colors.Error => "d-btn-error",
            _ => "d-btn-neutral",
        };

    [Parameter]
    public Variants Variant { get; set; } = Variants.Default;

    protected static string GetVariant(Variants style) =>
        style switch
        {
            Variants.Default => "",
            Variants.Outline => "d-btn-outline",
            Variants.Dash => "d-btn-dash",
            Variants.Soft => "d-btn-soft",
            Variants.Ghost => "d-btn-ghost",
            Variants.Link => "d-btn-link",
            _ => "",
        };

    [Parameter]
    public Behaviors Behavior { get; set; } = Behaviors.Default;

    protected static string GetBehavior(Behaviors behavior) =>
        behavior switch
        {
            Behaviors.Default => "",
            Behaviors.Active => "d-btn-active",
            Behaviors.Disabled => "d-btn-disabled",
            _ => "",
        };

    [Parameter]
    public Modifiers Modifier { get; set; } = Modifiers.None;

    protected static string GetModifier(Modifiers modifier) =>
        modifier switch
        {
            Modifiers.None => "expr",
            Modifiers.Wide => "d-btn-wide",
            Modifiers.Block => "d-btn-block",
            Modifiers.Square => "d-btn-square",
            Modifiers.Circle => "d-btn-circle",
            _ => "",
        };

    [Parameter]
    public bool Loading { get; set; } = false;

    protected bool IsLoading => Loading;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected bool Disabled => Behavior == Behaviors.Disabled;

    private protected string? Classes =>
        TwMerge.Merge(
            Type,
            GetVariant(Variant),
            GetColor(Color),
            GetSize(Size),
            GetBehavior(Behavior),
            GetModifier(Modifier)
        );
}
