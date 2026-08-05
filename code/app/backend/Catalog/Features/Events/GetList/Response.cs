using Catalog.Features.Events.Dtos;

namespace Catalog.Features.Events.GetList;

/// <summary>List of events matching the filter criteria.</summary>
public sealed record GetEventsListResponse(List<EventDto> Events);
