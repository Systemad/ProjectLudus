using CommunityToolkit.Mvvm.ComponentModel;

namespace Ludus.Client.Features.Loading;

public partial class LoadingService : ObservableObject
{
    [ObservableProperty]
    private bool _isLoading;
}
