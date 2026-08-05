using Catalog.Features.Events.Dtos;

namespace Catalog.Features.Events.GetById;

/// <summary>Event details including associated games.</summary>
public sealed record GetEventByIdResponse(EventDto Event);
