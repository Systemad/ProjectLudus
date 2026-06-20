using CatalogAPI.Features.Events.Dtos;

namespace CatalogAPI.Features.Events.GetById;

/// <summary>Event details including associated games.</summary>
public sealed record GetEventByIdResponse(EventDto Event);
