using ApiSdk;
using ApiSdk.Models;
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

    public List<Game>? Games { get; set; }

    [Inject]
    public ApiClient api { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var games = await api.Api.Games.Top.GetAsync(param =>
        {
            param.QueryParameters.PageNumber = 1;
            param.QueryParameters.PageSize = 20;
        });
        Games = games;
        //var games = await api.Api.Games.Top
        await base.OnInitializedAsync();
    }
}
