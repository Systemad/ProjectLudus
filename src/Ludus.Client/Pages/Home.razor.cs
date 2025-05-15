using Microsoft.AspNetCore.Components;

namespace Ludus.Client.Pages;

public partial class Home : ComponentBase
{
    public IReadOnlyCollection<string> SelectedValues = ["Milk", "Cafe Latte"];
    public bool ReadOnly;

    int _papers = 20;
    public string TextValue { get; set; }

    bool _expanded = true;

    private void OnExpandCollapseClick()
    {
        _expanded = !_expanded;
    }

    public GetTopRatedGamesResponse? RatedGames { get; set; }

    [Inject]
    public IGamesApi GamesApi { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await GamesApi.Top(ps: 20, pn: 1);
        /*
        var response = await Client.Api.Games.Top.GetAsync(param =>
        {
            param.QueryParameters.Ps = 20;
            param.QueryParameters.Pn = 1;
        });
        */
        RatedGames = response.Content;
        await base.OnInitializedAsync();
    }
}
