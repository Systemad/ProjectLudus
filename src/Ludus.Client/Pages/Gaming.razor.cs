using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.Kiota.Abstractions;
using MudBlazor;

namespace Ludus.Client.Pages;

public partial class Gaming : ComponentBase
{
    [Parameter]
    public long Id { get; set; }

    [Inject]
    public IGamesApi GamesApi { get; set; }

    [Inject]
    public ICollectionApi CollectionApi { get; set; }

    public Game? Game { get; set; }

    private UpsertGameCollectionQuery EditModel = new();

    public GameCollectionDto? Collection;

    private DateTime? _startDate;
    private DateTime? _endDate;
    private bool isLoading = true;

    private int selectedValueRating = 0;

    protected override async Task OnInitializedAsync()
    {
        EditModel.GameId = Id;
        var gameResponse = await GamesApi.GamesGET(Id);
        Game = gameResponse.Content;
        var response = await CollectionApi.CollectionGET2(Id);

        if (response.IsSuccessful)
        {
            Collection = response.Content;
            UpdateEditModel(response.Content);
        }
        else
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Collection = null;
                //Console.WriteLine(e);
            }
        }
        /*
        try
        {
            var response = await CollectionApi.CollectionGET2(Id);
            Collection = response;
            UpdateEditModel(response);
        }
        catch (ApiException e)
        {
            if (e.ResponseStatusCode == (int)HttpStatusCode.NotFound)
            {
                Collection = null;
                //Console.WriteLine(e);
            }
        }
*/
        isLoading = false;
        await base.OnInitializedAsync();
    }

    private async Task HandleStatusSelectedValueChanged(int val)
    {
        EditModel.Status = val;
        await HandleGameCollectionUpdateAsync();
    }

    private async Task HandleSelectedValueChanged(int val)
    {
        EditModel.Rating = val;
        await HandleGameCollectionUpdateAsync();
    }

    private async Task HandleGameCollectionUpdateAsync()
    {
        try
        {
            var response = await CollectionApi.CollectionPUT(EditModel);
            if (response.IsSuccessful)
            {
                UpdateEditModel(response.Content);

                StateHasChanged();
            }
        }
        catch (ApiException e)
        {
            Console.WriteLine(e);
            //throw;
        }
    }

    private void UpdateEditModel(GameCollectionDto collection)
    {
        EditModel = new UpsertGameCollectionQuery
        {
            EndDate = collection.EndDate,
            GameId = collection.GameId,
            Notes = collection.Notes,
            Rating = collection.Rating,
            StartDate = collection.StartDate,
            Status = collection.Status,
        };
        if (collection is { StartDate: { } start, EndDate: { } end })
        {
            _startDate = start.LocalDateTime;
            _endDate = end.LocalDateTime;
        }
        StateHasChanged();
    }
}
