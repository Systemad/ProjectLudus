namespace SyncService.Features.Seeding;

public interface ISeederService
{
    Task BeginSeedAsync(CancellationToken cancellationToken);
}
