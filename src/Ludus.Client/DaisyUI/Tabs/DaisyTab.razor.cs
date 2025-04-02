using Microsoft.AspNetCore.Components;

namespace Ludus.Client.DaisyUI.Tabs;

public partial class DaisyTab : DaisyComponentBase
{
    [CascadingParameter]
    public DaisyTabs Tabs { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Value { get; set; } = default!;

    [Parameter]
    public RenderFragment? TabContent { get; set; }

    [Parameter]
    public RenderFragment? Content { get; set; }

    public int Index { get; set; }

    protected override void OnInitialized()
    {
        Tabs.RegisterTab(this);
    }

    private Task SelectActiveTab() => Tabs.SetActiveTab(Value);

    public void Dispose()
    {
        Tabs.UnregisterTab(this);
    }

    private protected string? Classes => TwMerge.Merge();
}
