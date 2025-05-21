using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Ludus.Client.Pages;

public class SearchFilters
{
    public string TextValue { get; set; }
    public IEnumerable<GenreFilter> _selectedGenres = new HashSet<GenreFilter>();
    public IEnumerable<ThemeFilter> _selectedThemes = new HashSet<ThemeFilter>();
    public IEnumerable<PlatformFilter> _selectedPlatforms = new HashSet<PlatformFilter>();
    public IEnumerable<GameTypeFilter> _selectedGameTypes = new HashSet<GameTypeFilter>();
    public IEnumerable<GameModeFilter> _selectedGameModes = new HashSet<GameModeFilter>();
    public IEnumerable<GameEngineFilter> _selectedGameEngines = new HashSet<GameEngineFilter>();
    public IEnumerable<PlayerPerspectiveFilter> _selectedPlayerPerspectives =
        new HashSet<PlayerPerspectiveFilter>();
}

public partial class AllGames : ComponentBase
{
    [Inject]
    public IGamesApi GamesApi { get; set; }

    public SearchFilters SearchFilters = new SearchFilters();

    public GetFiltersResponse? Filters = new GetFiltersResponse();

    public GetSearchGamesResult? FetchedGames = new GetSearchGamesResult();

    private List<GameDTO> Games = new();
    private bool isLoading = false;
    private const int PageSize = 40;
    private int currentPage = 1;

    protected override async Task OnInitializedAsync()
    {
        var filers = await GamesApi.Filters();
        Filters = filers.Content;

        var defaultGameFilter = Filters?.GameTypes?.FirstOrDefault(x => x.Id == 0);
        SearchFilters._selectedGameTypes = new HashSet<GameTypeFilter> { defaultGameFilter };

        await FetchGamesAsync(1);
        await base.OnInitializedAsync();
    }

    public async Task SearchGamesAsync()
    {
        Games.Clear();
        FetchedGames = null;
        currentPage = 1;

        await FetchGamesAsync(1);
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
        var searchedResponse = await GamesApi.Search(
            pn: page,
            ps: 40,
            name: SearchFilters.TextValue,
            genreid: SearchFilters._selectedGenres.Select(x => x.Id),
            platformid: SearchFilters._selectedPlatforms.Select(x => x.Id),
            themeid: SearchFilters._selectedThemes.Select(x => x.Id),
            gamemodeid: SearchFilters._selectedGameModes.Select(x => x.Id),
            gametypeid: SearchFilters._selectedGameModes.Select(x => x.Id),
            ppsid: SearchFilters._selectedPlayerPerspectives.Select(x => x.Id)
        );

        if (searchedResponse.Content is not null)
        {
            if (page == 1)
            {
                Games = searchedResponse.Content.Games.ToList();
            }
            else
            {
                Games.AddRange(searchedResponse.Content.Games);
            }

            FetchedGames = searchedResponse.Content;
        }
    }
}
