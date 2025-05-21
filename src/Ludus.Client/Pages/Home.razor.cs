using Ludus.Client.Features.Loading;
using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class Home : ComponentBase
{
    public GetTopRatedGamesResponse? FetchedGames { get; set; }

    [Inject]
    public IGamesApi GamesApi { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    private List<GameDTO> Games = new();
    private int currentPage = 1;
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await FetchGamesAsync(1);
        await base.OnInitializedAsync();
    }

    public async Task LoadMoreGamesAsync()
    {
        if (FetchedGames is not null && !FetchedGames.IsLastPage)
        {
            await FetchGamesAsync((int)FetchedGames.PageNumer + 1);
        }
    }

    public async Task FetchGamesAsync(int page)
    {
        var response = await GamesApi.Top(ps: 40, pn: page);

        if (response.Content is not null)
        {
            if (page == 1)
            {
                Games = response.Content.Games.ToList();
            }
            else
            {
                Games.AddRange(response.Content.Games);
            }

            FetchedGames = response.Content;
        }
    }

    [Inject]
    public LoadingService LoadingService { get; set; }

    private void NavigateToGame(long id)
    {
        NavigationManager.NavigateTo($"/games/{id}", false);
        LoadingService.IsLoading = false;
    }
}
