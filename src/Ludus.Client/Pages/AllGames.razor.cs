using Ludus.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class AllGames : ComponentBase
{
    [Inject]
    public LudusClient Client { get; set; }
    public string TextValue { get; set; }

    public GetFiltersResponse? Filters = new GetFiltersResponse();

    public GetSearchGamesResult? FetchedGames = new GetSearchGamesResult();

    public IEnumerable<long> _selectedGenreIds = new HashSet<long>();

    public IEnumerable<long> _selectedThemeIds = new HashSet<long>();

    public IEnumerable<GenreFilter> _selectedGenres = new HashSet<GenreFilter>();

    public string value { get; set; } = "Nothing selected";

    private int _selectedPage = 1;
    private int _pageCount = 1;

    protected override async Task OnInitializedAsync()
    {
        var filers = await Client.Api.Games.Filters.GetAsync();
        if (filers is not null)
        {
            Filters = filers;
        }
        await base.OnInitializedAsync();
    }

    private string GetMultiSelectionText(List<string?>? selectedIds)
    {
        if (selectedIds == null || Filters.Genres == null)
            return string.Empty;

        var selectedNames = Filters
            .Genres.Where(g => selectedIds.Contains(g.Id.ToString()))
            .Select(g => g.Name)
            .ToList();

        return $"{string.Join(", ", selectedNames)}";
    }

    private string GetMultiSelectionTextTheme(List<string?>? selectedIds)
    {
        if (selectedIds == null || Filters.Themes == null)
            return string.Empty;

        var selectedNames = Filters
            .Themes.Where(g => selectedIds.Contains(g.Id.ToString()))
            .Select(g => g.Name)
            .ToList();

        return $"{string.Join(", ", selectedNames)}";
    }

    public async Task FetchGamesAsync()
    {
        var fetchedGames = await Client.Api.Games.Search.GetAsync(request =>
        {
            request.QueryParameters.Pn = 1;
            request.QueryParameters.Ps = 20;
            request.QueryParameters.Name = TextValue;
            request.QueryParameters.Genreid = _selectedGenres.Select(x => x.Id).ToArray();
        });
        Console.WriteLine(string.Join(", ", fetchedGames.Games.Select(x => x.Name)));
        FetchedGames = fetchedGames;
        _pageCount = (int)FetchedGames.PageCount;
    }
}
