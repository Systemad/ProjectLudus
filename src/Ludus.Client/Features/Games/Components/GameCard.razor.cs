using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ludus.Client.Features.Games.Components;

public partial class GameCard : ComponentBase
{
    [Parameter]
    public GameDTO Game { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    void NavigateToDetails()
    {
        // Do navigation or some action
        NavigationManager.NavigateTo($"/games/{Game.Id}", forceLoad: true);
    }

    private bool _open;

    private Task OpenDialogAsync()
    {
        var parameters = new DialogParameters<GameCardActivityDialog>() { { x => x.Game, Game } };
        var options = new DialogOptions { CloseOnEscapeKey = true };

        return DialogService.ShowAsync<GameCardActivityDialog>(
            "Simple Dialog",
            parameters,
            options
        );
    }

    private async Task OnOpen()
    {
        _open = true;
        await Task.Delay(2500);
        _open = false;
    }
}
