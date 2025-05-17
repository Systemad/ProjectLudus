using System.Net;
using Ludus.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;

namespace Ludus.Client.Features.Games.Components;

public partial class GameCardActivityDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public GameDTO? Game { get; set; } = null!; // new GameDTO();

    public GameCollectionDto Collection { get; set; }

    [Inject]
    public ICollectionApi CollectionApi { get; set; }

    private GameStatus selectedGameStatus = GameStatus.None;

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        if (Game?.Id is { } id)
        {
            try
            {
                var collection = await CollectionApi.CollectionGET2(id); //await Client.Api.Collection[330684].GetAsync();
                Collection = collection.Content;
            }
            catch (ApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("ITEM NOT FOUND");
                }
                //throw;
            }
            //Collection = collection;
            //selectedGameStatus = (GameStatus)collection.Status;
            StateHasChanged();
        }
        await base.OnInitializedAsync();
    }
}
