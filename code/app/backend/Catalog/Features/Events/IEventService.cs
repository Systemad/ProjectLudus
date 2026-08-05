using Catalog.Features.Events.Dtos;

namespace Catalog.Features.Events;

public interface IEventService
{
    Task<List<EventDto>> GetListAsync(
        int? year,
        int? month,
        string? status,
        int? limit,
        CancellationToken ct
    );
    Task<EventDto?> GetByIdAsync(long id, CancellationToken ct);
}
