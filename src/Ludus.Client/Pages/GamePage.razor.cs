using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class GamePage : ComponentBase
{
    [Parameter]
    public long? Id { get; set; }

    bool _expanded = true;

    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }

    public string TargetIcon =
        @"<circle cx=""12"" cy=""12"" r=""10""/><line x1=""22"" x2=""18"" y1=""12"" y2=""12""/><line x1=""6"" x2=""2"" y1=""12"" y2=""12""/><line x1=""12"" x2=""12"" y1=""6"" y2=""2""/><line x1=""12"" x2=""12"" y1=""22"" y2=""18""/>";
}
