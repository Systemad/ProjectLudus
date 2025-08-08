using WebAPI.Features.Common.Games.Models;

namespace WebAPI.Features.Common.Games.Mappers;

public static class GameMetadataMapper
{
    public static IEnumerable<GameMetadataDto> MapGamesToMetadatas(
        IEnumerable<long> games,
        HashSet<long> wishlistedSet,
        HashSet<long> hypedSet
    )
    {
        var gameDtos = games
            .Select(g => MapToGameMetadata(g, wishlistedSet.Contains(g), hypedSet.Contains(g)))
            .ToList();

        return gameDtos;
    }   

    public static GameMetadataDto MapToGameMetadata(long gameId, bool isWishlisted, bool isHyped)
    {
        return new GameMetadataDto()
        {
            Id =  gameId,
            IsWishlisted = isWishlisted,
            IsHyped = isHyped,
        };
    }
}
