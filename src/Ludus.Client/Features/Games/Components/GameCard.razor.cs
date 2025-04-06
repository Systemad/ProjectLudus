using ApiSdk.Models;
using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Features.Games.Components;

public partial class GameCard : ComponentBase
{
    [Parameter]
    public Game Game { get; set; }

    private bool _open;

    private async Task OnOpen()
    {
        _open = true;
        await Task.Delay(2500);
        _open = false;
    }
}
