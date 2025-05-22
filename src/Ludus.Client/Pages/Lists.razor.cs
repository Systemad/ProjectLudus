using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Ludus.Client.Pages;

public partial class Lists : ComponentBase
{
    [Inject]
    public IListApi ListApi { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public List<UserGameListDto> FetchedList = new List<UserGameListDto>();

    private bool _visible;

    private readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    public CreateListForm model = new CreateListForm();

    public class CreateListForm
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Name length can't be more than 20.")]
        public string Name { get; set; }

        [Required]
        public bool Public { get; set; }
    }

    bool success;

    private void OpenDialog() => _visible = true;

    private void Submit() => _visible = false;

    protected override async Task OnInitializedAsync()
    {
        var lists = await ListApi.ListAll(true);
        if (lists.Content is not null)
        {
            FetchedList = lists.Content.ToList();
        }
        await base.OnInitializedAsync();
    }

    private void OnValidSubmit(EditContext context)
    {
        success = true;
        StateHasChanged();
    }

    private async Task OnValidSubmit1(EditContext context)
    {
        var response = await ListApi.ListPOST(
            new CreateListQuery() { Name = model.Name, Public = model.Public }
        );
        if (response.IsSuccessful)
        {
            success = true;
            Submit();
            StateHasChanged();
        }

        model = new CreateListForm();
    }

    private void NavigateToList(Guid id)
    {
        NavigationManager.NavigateTo($"/list/{id}");
    }
}
