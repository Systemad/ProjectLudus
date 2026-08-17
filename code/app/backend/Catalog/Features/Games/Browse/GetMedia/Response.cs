namespace Catalog.Features.Games.Browse.GetMedia;

public sealed record GameMediaDto
{
    public required IReadOnlyList<string> Screenshots { get; init; }

    public required IReadOnlyList<GameMediaVideoDto> Videos { get; init; }
}

public sealed record GameMediaVideoDto(string Name, string VideoId);

public sealed record GetGameMediaResponse(GameMediaDto Game);
