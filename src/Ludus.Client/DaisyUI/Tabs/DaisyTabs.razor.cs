using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Tabs;

public partial class DaisyTabs : DaisyComponentBase
{
    private static string Type = "d-tabs";

    public enum Variants
    {
        Normal,
        Border,
        Lift,
        Box,
    }

    public enum Sizes
    {
        XS,
        SM,
        MD,
        LG,
        XL,
    }

    private int _tabIndex = 0;

    public Dictionary<string, DaisyTab> Tabs { get; private set; } = new();

    public string? ActiveTabId { get; private set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public Sizes Size { get; set; } = Sizes.MD;

    protected static string GetSize(Sizes size) =>
        size switch
        {
            Sizes.XS => "d-tabs-xs",
            Sizes.SM => "d-tabs-sm",
            Sizes.MD => "d-tabs-md",
            Sizes.LG => "d-tabs-lg",
            Sizes.XL => "d-tabs-xl",
            _ => "d-tabs-md",
        };

    [Parameter]
    public Variants Variant { get; set; } = Variants.Normal;

    protected static string GetVariant(Variants variant) =>
        variant switch
        {
            Variants.Normal => "d-tabs",
            Variants.Border => "d-tabs-border",
            Variants.Lift => "d-tabs-lift",
            Variants.Box => "d-tabs-box",
            _ => "d-tabs",
        };

    private protected string? Classes => TwMerge.Merge(Type, GetVariant(Variant), GetSize(Size));

    protected override void OnInitialized()
    {
        ActiveTabId = Tabs.Keys.FirstOrDefault();
    }

    public void RegisterTab(DaisyTab tab)
    {
        tab.Index = _tabIndex++;
        Tabs[tab.Value] = tab;

        if (ActiveTabId is null)
        {
            ActiveTabId = tab.Value;
        }

        StateHasChanged();
    }

    public void UnregisterTab(DaisyTab tab)
    {
        if (Tabs.ContainsKey(tab.Value))
        {
            Tabs.Remove(tab.Value);
            if (ActiveTabId == tab.Value)
            {
                ActiveTabId = Tabs.Keys.FirstOrDefault();
            }

            StateHasChanged();
        }
    }

    public async Task SetActiveTab(string tabId)
    {
        if (ActiveTabId != tabId && Tabs.ContainsKey(tabId))
        {
            ActiveTabId = tabId;
            await InvokeAsync(StateHasChanged);
        }
    }
}
