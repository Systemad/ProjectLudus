using Microsoft.AspNetCore.Components;

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

    public string value { get; set; } = "Nothing selected";

    private int _selectedPage = 1;
    private int _pageCount = 1;

    protected override async Task OnInitializedAsync()
    {
        var filers = await GamesApi.Filters(); //Client.Api.Games.Filters.GetAsync();
        Filters = filers.Content;

        await FetchGamesAsync();
        await base.OnInitializedAsync();
    }

    public async Task FetchGamesAsync()
    {
        var defaultGameFilter = Filters?.GameTypes?.FirstOrDefault(x => x.Id == 1001);
        SearchFilters._selectedGameTypes = new HashSet<GameTypeFilter> { defaultGameFilter };
        var hey = await GamesApi.Search(
            pn: 1,
            ps: 20,
            name: SearchFilters.TextValue,
            genreid: SearchFilters._selectedGenres.Select(x => x.Id),
            platformid: SearchFilters._selectedPlatforms.Select(x => x.Id),
            themeid: SearchFilters._selectedThemes.Select(x => x.Id),
            gamemodeid: SearchFilters._selectedGameModes.Select(x => x.Id),
            ppsid: SearchFilters._selectedPlayerPerspectives.Select(x => x.Id)
        );
        /*
        var fetchedGames = await Client.Api.Games.Search.GetAsync(request =>
        {
            request.QueryParameters.Pn = 1;
            request.QueryParameters.Ps = 20;
            request.QueryParameters.Name = SearchFilters.TextValue;
            request.QueryParameters.Genreid = SearchFilters
                ._selectedGenres.Select(x => x.Id)
                .ToArray();
            request.QueryParameters.Platformid = SearchFilters
                ._selectedPlatforms.Select(x => x.Id)
                .ToArray();
            request.QueryParameters.Themeid = SearchFilters
                ._selectedThemes.Select(x => x.Id)
                .ToArray();
            request.QueryParameters.Gamemodeid = SearchFilters
                ._selectedGameModes.Select(x => x.Id)
                .ToArray();
        });
        */
        FetchedGames = hey.Content;
        _pageCount = (int)FetchedGames.PageCount;
    }
}
