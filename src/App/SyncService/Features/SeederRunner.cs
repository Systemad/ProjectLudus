namespace SyncService.Features;

public class SeederRunner
{
    private readonly IEnumerable<ISeederService> _seederServices;

    public SeederRunner(IEnumerable<ISeederService> seederServices)
    {
        _seederServices = seederServices;
    }
    
    public async Task RunAllAsync()
    {
        foreach (var service in _seederServices)
        {
            await service.SeedCompaniesAsync();
        }
    }
}