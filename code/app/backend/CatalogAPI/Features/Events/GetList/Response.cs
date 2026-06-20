using CatalogAPI.Features.Events.Dtos;

namespace CatalogAPI.Features.Events.GetList;

/// <summary>List of events matching the filter criteria.</summary>
public sealed record GetEventsListResponse(List<EventDto> Events);
