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

    protected override async Task OnInitializedAsync()
    {
        var collections = await CollectionApi.CollectionGET();
        if (collections.IsSuccessful)
        {
            GameCollections = collections.Content;
            Console.WriteLine(collections.Content.TotalItemCount);
        }
        await base.OnInitializedAsync();
    }

    private void NavigateToGame(long id)
    {
        NavigationManager.NavigateTo($"/gaming/{id}", false);
        LoadingService.IsLoading = false;
    }
}
