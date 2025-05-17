using System.Net;
using Ludus.Client.Features.Loading;
using Microsoft.AspNetCore.Components;
using Refit;

namespace Ludus.Client.Pages;

public partial class Gaming : ComponentBase
{
    [Parameter]
    public long Id { get; set; }

    [Inject]
    public IGamesApi GamesApi { get; set; }

    [Inject]
    public ICollectionApi CollectionApi { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public LoadingService LoadingService { get; set; }

    public GameDetail? GameDetail { get; set; }

    private UpsertGameCollectionQuery EditModel = new();

    public GameCollectionDto? Collection;

    private bool GameNotStarted => Collection?.Status == 4;
    private DateTime? _startDate = DateTime.UtcNow;
    private DateTime? _endDate = DateTime.UtcNow;
    private bool isLoading = true;
    private bool isDescriptionExpanded = false;

    private int selectedValueRating = 0;

    private bool _ratingOpen;

    private void ToggleRatingOpenOpen() => _ratingOpen = !_ratingOpen;

    protected override async Task OnParametersSetAsync()
    {
        GameDetail = null;
        Collection = null;
        EditModel = new();
        EditModel.GameId = Id;
        var gameResponse = await GamesApi.GamesGET(Id);
        GameDetail = gameResponse.Content;
        var response = await CollectionApi.CollectionGET2(Id);

        try
        {
            if (response.IsSuccessful)
            {
                Collection = response.Content;
                UpdateEditModel(response.Content);
            }
        }
        catch (Exception e)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Collection = null;
                //Console.WriteLine(e);
            }
            Console.WriteLine(e);
            //throw;
        }

        isLoading = false;
        LoadingService.IsLoading = false;
        await base.OnInitializedAsync();
    }

    private async Task HandleStatusSelectedValueChanged(int val)
    {
        EditModel.Status = val;
        await HandleGameCollectionUpdateAsync();
    }

    private int? activeVal;

    private string LabelText =>
        (activeVal ?? EditModel.Rating) switch
        {
            1 => "1",
            2 => "2",
            3 => "3",
            4 => "4",
            5 => "5",
            _ => "",
        };

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
                Collection = response.Content;
                UpdateEditModel(response.Content);
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

    private void NavigateToGame(long id)
    {
        NavigationManager.NavigateTo($"/gaming/{id}", false);
        LoadingService.IsLoading = false;
    }
}
