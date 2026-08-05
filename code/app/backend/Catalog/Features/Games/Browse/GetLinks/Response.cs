using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Browse.GetLinks;

public sealed record GetGameLinksResponse(List<WebsiteDto> Websites);
