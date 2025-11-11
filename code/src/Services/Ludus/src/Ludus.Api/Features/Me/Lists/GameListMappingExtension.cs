using Ludus.Api.Features.Common.Games.Models;
using Ludus.Api.Features.Common.Users.Models;

namespace Me.Lists;

public static class GameListMappingExtension
{
    public static GameListItemDto MapToGameListItemDto(this GameListItem entity, GameDto game)
    {
        return new GameListItemDto
        {
            Id = entity.Id,
            AddedAt = entity.AddedAt,
            Game = game,
            GameListId = entity.GameListId
        };
    }
    
    public static GameListDto MapToGameListDto(this GameList entity, List<GameListItemDto> items)
    {
        return new GameListDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Public = entity.Public,
            TotalItems = items.Count,
            Created = entity.CreatedAt,
            Items = items
        };
    }
}