using Catalog.Features.Games.Browse.GetHero;

namespace Catalog.Features.Games.Browse.GetHeroes;

public sealed record GetGameHeroesResponse(IReadOnlyList<GameHeroDto> Games);
