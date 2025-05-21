using Ludus.Client.Features.Loading;
using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class Library : ComponentBase
{
    [Inject]
    public LoadingService LoadingService { get; set; }

    [Inject]
    public IGamesApi GamesApi { get; set; }

    [Inject]
    public ICollectionApi CollectionApi { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public GetGameCollectionPreviewsResponse? GameCollections =
        new GetGameCollectionPreviewsResponse();

    private List<GameCollectionPreviewDto> SortedList = new();
    private int SortedByStatus = 1;

    protected override async Task OnInitializedAsync()
    {
        var collections = await CollectionApi.CollectionGET();
        if (collections.IsSuccessful)
        {
            GameCollections = collections.Content;
            SortedList = collections.Content.Entries.ToList();
            Console.WriteLine(collections.Content.TotalItemCount);
        }
        await base.OnInitializedAsync();
    }

    private void HandleStatusSelectedValueChanged(int val)
    {
        SortedByStatus = val;
        if (GameCollections is not null)
        {
            if (val == 0)
            {
                SortedList = GameCollections.Entries.ToList();
            }
            else
            {
                SortedList = GameCollections.Entries.Where(x => x.Status == val).ToList();
            }
        }
        //await HandleGameCollectionUpdateAsync();
    }

    private void NavigateToGame(long id)
    {
        NavigationManager.NavigateTo($"/games/{id}", false);
        LoadingService.IsLoading = false;
    }
}
