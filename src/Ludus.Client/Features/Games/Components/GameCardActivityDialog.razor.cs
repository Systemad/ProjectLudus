using Ludus.Client.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace Ludus.Client.Features.Games.Components;

public partial class GameCardActivityDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public Game Game { get; set; } = new Game();

    private GameStatus GStatus { get; set; } = GameStatus.Played;

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
}
