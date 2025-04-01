using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class GamePage : ComponentBase
{
    [Parameter]
    public string? GameId { get; set; }
}
